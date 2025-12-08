const minPriceInput = document.getElementById('min-price');
const maxPriceInput = document.getElementById('max-price');
const priceValuesDisplay = document.getElementById('price-values');

const inventoryContainer = document.querySelector('.sc-inv-grid-container');
const sortSelect = document.getElementById('sort-options');
const applyFilterButton = document.getElementById('apply-filter');

const DEFAULT_MIN_PRICE = 0;
const DEFAULT_MAX_PRICE = 500000;

// ❗ ИСПРАВЛЕНИЕ: mySearch объявлена глобально и не переопределяется локально.
let mySearch;


// ----------------------------------------------------------------------
// --- ФУНКЦИИ ПОИСКА ---
// ----------------------------------------------------------------------

function insertMark(str, pos, len) {
    return str.slice(0, pos) + '<mark>' + str.slice(pos, pos + len) + '</mark>' + str.slice(pos + len);
}

function triggerSearch() {
    // ❗ Защита от запуска до инициализации.
    if (!mySearch) return;

    let val = mySearch.value.trim().toLowerCase();

    // 1. Выбираем все карточки инвентаря, которые были загружены после фильтра
    let items = document.querySelectorAll('.sc-inv-card');

    items.forEach(function (item) {
        // 2. Получаем название товара
        let itemNameElement = item.querySelector('.sc-inv-card-title');
        // Защита от случая, если нет элемента с названием
        if (!itemNameElement) return;

        let originalName = itemNameElement.innerText;
        let itemName = originalName.toLowerCase();

        // 3. Логика скрытия: если введенный текст не найден, скрываем
        if (itemName.search(val) === -1) {
            item.classList.add('hide');
        } else {
            // Если найдено, показываем
            item.classList.remove('hide');
        }

        // 4. Логика подсветки
        if (val.length > 0 && itemName.search(val) !== -1) {
            // Очищаем предыдущую подсветку, чтобы избежать дублирования <mark>
            itemNameElement.innerHTML = originalName;
            itemNameElement.innerHTML = insertMark(originalName, itemName.search(val), val.length);
        } else {
            // Очищаем подсветку, если текст не найден или поле ввода пустое
            itemNameElement.innerHTML = originalName;
        }
    });


    const currentSort = sortSelect ? sortSelect.value : 'default';
    if (currentSort !== 'default') {
        triggerSort(currentSort);
    }
}

// ----------------------------------------------------------------------
// --- ФУНКЦИИ ЦЕНЫ ---
// ----------------------------------------------------------------------

function updatePriceValues() {
    const minVal = minPriceInput.value;
    const maxVal = maxPriceInput.value;
    priceValuesDisplay.innerText = `${minVal} - ${maxVal}`;
}

// ----------------------------------------------------------------------
// --- ФУНКЦИИ СОХРАНЕНИЯ СОСТОЯНИЯ ---
// ----------------------------------------------------------------------

function saveFilterState() {
    const checkedCategories = document.querySelectorAll('.category-checkbox:checked');
    const categoryIds = Array.from(checkedCategories).map(cb => cb.value);

    const state = {
        min: minPriceInput.value,
        max: maxPriceInput.value,
        categories: categoryIds
    };
    sessionStorage.setItem('inventoryFilterState', JSON.stringify(state));
}

function loadFilterState() {
    const savedState = sessionStorage.getItem('inventoryFilterState');

    if (savedState) {
        const state = JSON.parse(savedState);

        if (minPriceInput && maxPriceInput) {
            minPriceInput.value = state.min;
            maxPriceInput.value = state.max;
            updatePriceValues();
        }

        if (state.categories && Array.isArray(state.categories)) {
            state.categories.forEach(id => {
                const checkbox = document.querySelector(`.category-checkbox[value="${id}"]`);
                if (checkbox) {
                    checkbox.checked = true;
                }
            });
        }

        applyFilter(false); // Применяем фильтр
    } else {
        applyFilter(false);
    }
}

// ----------------------------------------------------------------------
// --- ФУНКЦИИ ОТОБРАЖЕНИЯ И ФИЛЬТРАЦИИ ---
// ----------------------------------------------------------------------

function dataDisplay(responseData) {
    if (!inventoryContainer) return;

    inventoryContainer.innerHTML = '';

    const data = responseData.data || responseData;
    const noInventoryMessage = '<p>В настоящее время инвентарь не найден.</p>';

    if (!data || data.length === 0) {
        inventoryContainer.innerHTML = noInventoryMessage;
    } else {
        data.forEach(item => {
            const inventoryItem = `
               <a href="/Inventory/PageOfInventory/${item.id}" class="sc-inv-card inventory-item">
                <img src="${item.pathImg}" alt="${item.name || 'Инвентарь'}" class="sc-inv-card-image" />
                <div class="sc-inv-card-overlay">
                    <div class="sc-inv-card-title">${item.name}</div>
                    <p class="sc-inv-card-description">
                        Количество в наличии: <span class="item-count">${item.count || 0}</span> <br />
                        Цена: <span class="item-price">${item.price.toFixed(2)} ₽</span>
                    </p>
                </div>
           </a>`;
            inventoryContainer.innerHTML += inventoryItem;
        });


    }
}


function applyFilter(shouldSaveState = true) {
    const priceMin = minPriceInput ? minPriceInput.value : DEFAULT_MIN_PRICE;
    const priceMax = maxPriceInput ? maxPriceInput.value : DEFAULT_MAX_MAX;


    const checkedCategories = document.querySelectorAll('.category-checkbox:checked');

    const categoryIds = Array.from(checkedCategories).map(cb => cb.value);


    const filterData = {
        PriceMin: parseFloat(priceMin),
        PriceMax: parseFloat(priceMax),
        CategoryIds: categoryIds
    };

    fetch('/Inventory/Filter', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(filterData),
    })
        .then((response) => {
            if (!response.ok) {
                return response.json().then(errorData => {
                    throw new Error(errorData.Description || `Ошибка HTTP: ${response.status}`);
                });
            }
            return response.json();
        })
        .then((data) => {
            dataDisplay(data);
            if (shouldSaveState) {
                saveFilterState();
            }



            // 1. Применяем поиск, если в поле есть текст
            if (mySearch && mySearch.value.trim() !== '') {
                triggerSearch();
            }


            const currentSort = sortSelect ? sortSelect.value : 'default';
            if (currentSort !== 'default') {
                triggerSort(currentSort);
            }

        })
        .catch((error) => {
            if (inventoryContainer) {
                inventoryContainer.innerHTML = `<h2>Ошибка при загрузке: ${error.message}</h2>`;
            }
            console.error(error);
        });
}

// ----------------------------------------------------------------------
// --- ФУНКЦИИ СОРТИРОВКИ ---
// ----------------------------------------------------------------------

function triggerSort(sortOption) {
    if (!inventoryContainer) return;

    // Выбираем все карточки, включая скрытые, чтобы сортировать весь набор
    const items = Array.from(inventoryContainer.querySelectorAll('.sc-inv-card'));

    items.sort((a, b) => {
 
        const getCount = (element) => parseFloat(element.querySelector('.item-count').textContent.trim()) || 0;

        const getPrice = (element) => parseFloat(element.querySelector('.item-price').textContent.replace(' ₽', '').trim()) || 0;

        switch (sortOption) {
            case 'price-asc':
                return getPrice(a) - getPrice(b);
            case 'price-desc':
                return getPrice(b) - getPrice(a);
            case 'count-asc':
                return getCount(a) - getCount(b);
            case 'count-desc':
                return getCount(b) - getCount(a);
            default:
                return 0;
        }
    });

    items.forEach(item => inventoryContainer.appendChild(item));
}


// ----------------------------------------------------------------------
// --- ИНИЦИАЛИЗАЦИЯ И ОБРАБОТЧИКИ СОБЫТИЙ ---
// ----------------------------------------------------------------------

if (minPriceInput && maxPriceInput) {
    updatePriceValues();
    minPriceInput.addEventListener('input', updatePriceValues);
    maxPriceInput.addEventListener('input', updatePriceValues);
}

if (sortSelect) {
    window.addEventListener('pageshow', () => {
        sortSelect.value = 'default';
    });
    sortSelect.addEventListener('change', () => {
        const sortOption = sortSelect.value;
        triggerSort(sortOption);


        if (mySearch && mySearch.value.trim() !== '') {
            triggerSearch();
        }
    });
}

if (applyFilterButton) {
    applyFilterButton.addEventListener('click', (e) => {
        e.preventDefault();
        applyFilter(true);
    });
}


document.addEventListener('DOMContentLoaded', function () {

    mySearch = document.querySelector('#mySearch');

    if (mySearch) {
        const clear = document.querySelector('.clear');
        if (clear) {
            clear.addEventListener('click', function () {
                mySearch.value = '';
                triggerSearch();
            });
        }

        // ❗ При вводе - запускаем поиск
        mySearch.oninput = triggerSearch;

        // Добавляем обработчик для кнопки поиска с классом .icon, если она есть
        const searchButton = document.querySelector('.icon');
        if (searchButton) {
            searchButton.addEventListener('click', triggerSearch);
        }
    }

    // Загружаем состояние фильтра при старте страницы
    loadFilterState();
});