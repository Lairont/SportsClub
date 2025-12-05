const minPriceInput = document.getElementById('min-price');
const maxPriceInput = document.getElementById('max-price');
const priceValuesDisplay = document.getElementById('price-values');

const inventoryContainer = document.querySelector('.sc-inv-grid-container');
const sortSelect = document.getElementById('sort-options');
const applyFilterButton = document.getElementById('apply-filter');

const DEFAULT_MIN_PRICE = 0;
const DEFAULT_MAX_PRICE = 500000;



function updatePriceValues() {
    const minVal = minPriceInput.value;
    const maxVal = maxPriceInput.value;
    priceValuesDisplay.innerText = `${minVal} - ${maxVal}`;
}


if (minPriceInput && maxPriceInput) {
    updatePriceValues();
    minPriceInput.addEventListener('input', updatePriceValues);
    maxPriceInput.addEventListener('input', updatePriceValues);
}

if (sortSelect) {
    window.addEventListener('pageshow', () => {
        sortSelect.value = 'default';
    });
}


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


        applyFilter(false);
    } else {

        applyFilter(false);
    }
}


function dataDisplay(responseData) {
    if (!inventoryContainer) return;

    inventoryContainer.innerHTML = '';

    const data = responseData.data || responseData;
    const noInventoryMessage = '<p>В настоящее время инвентарь не найден.</p>';

    if (!data || data.length === 0) {
        inventoryContainer.innerHTML = noInventoryMessage;
    } else {
        data.forEach(item => {   
            /*сейчас Details не работает но будет*/
            const inventoryItem = `
            <a href="/Inventory/Details/${item.id}" class="sc-inv-card inventory-item"> 
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

        const currentSort = sortSelect ? sortSelect.value : 'default';
        if (currentSort !== 'default') {
            triggerSort(currentSort);
        }
    }
}


function applyFilter(shouldSaveState = true) {
    const priceMin = minPriceInput ? minPriceInput.value : DEFAULT_MIN_PRICE;
    const priceMax = maxPriceInput ? maxPriceInput.value : DEFAULT_MAX_PRICE;


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
        })
        .catch((error) => {
            if (inventoryContainer) {
                inventoryContainer.innerHTML = `<h2>Ошибка при загрузке: ${error.message}</h2>`;
            }
            console.error(error);
        });
}

if (applyFilterButton) {
    applyFilterButton.addEventListener('click', (e) => {
        e.preventDefault();
        applyFilter(true);
    });
}

function triggerSort(sortOption) {
    if (!inventoryContainer) return;

    const items = Array.from(inventoryContainer.querySelectorAll('.sc-inv-card'));

    items.sort((a, b) => {
        const getCount = (element) => parseFloat(element.querySelector('.item-count').textContent.trim()) || 0;
        const getPrice = (element) => parseFloat(element.querySelector('.item-price').textContent.trim()) || 0;

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

if (sortSelect && inventoryContainer) {
    sortSelect.addEventListener('change', () => {
        const sortOption = sortSelect.value;
        triggerSort(sortOption);
    });
}

document.addEventListener('DOMContentLoaded', loadFilterState);