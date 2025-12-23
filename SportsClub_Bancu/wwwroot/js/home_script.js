document.addEventListener('DOMContentLoaded', () => {
    const cardInner = document.querySelector('.flip-card-inner');
    const cardContainer = document.getElementById('flipCard');
    const frontFace = document.getElementById('front');
    const backFace = document.getElementById('back');


    const services = [
        {
            title: "Доставка по всей стране",
            text: "Осуществляем перевозки в любой регион — от локальных выездов до междугородних марафонов и турниров. Чёткое соблюдение графика и отслеживание в реальном времени.",
            imgSrc: "/images/postrane.jpg",
            alt: "Доставка"
        },
        {
            title: "Любой объём инвентаря",
            text: "Перевозим как один мяч, так и полный комплект экипировки для крупного спортивного клуба: от гимнастических матов до хоккейных ворот и велотренажёров.",
            imgSrc: "/images/fint.jpg",
            alt: "Инвентарь"
        },
        {
            title: "Экспресс-доставка",
            text: "Нужно срочно доставить инвентарь к соревнованиям? Готовы выехать в течение 2 часов после заявки — без задержек и переплат.",
            imgSrc: "/images/distavka.jpg",
            alt: "Экспресс"
        },
        {
            title: "Полная страховка",
            text: "Весь инвентарь застрахован на полную стоимость. В редком случае повреждения — компенсация без бюрократии и задержек.",
            imgSrc: "/images/strohovka.jpg",
            alt: "Страховка"
        },
        {
            title: "Поддержка 24/7",
            text: "Наша команда на связи круглосуточно. Поможем с логистикой перед турниром, изменим маршрут или ответим на вопросы — в любое время.",
            imgSrc: "/images/poderka.jpg",
            alt: "Поддержка"
        }
    ];

    let currentFace = 'front'; 
    let currentIndex = 0;
    const totalServices = services.length;

    const generateContent = (service) => `
        <img src="${service.imgSrc}" alt="${service.alt}">
        <h3>${service.title}</h3>
        <p>${service.text}</p>
    `;

    const updateHiddenFace = () => {
        let nextIndex = (currentIndex + 1) % totalServices;
        let hiddenFace = (currentFace === 'front' ? backFace : frontFace);

        hiddenFace.innerHTML = generateContent(services[nextIndex]);
    };

    frontFace.innerHTML = generateContent(services[currentIndex]);
    updateHiddenFace();

    cardContainer.addEventListener('click', () => {
        cardInner.classList.toggle('flipped');

        currentFace = (currentFace === 'front' ? 'back' : 'front');

        currentIndex = (currentIndex + 1) % totalServices;
        setTimeout(updateHiddenFace, 400);
    });

});