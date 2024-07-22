document.addEventListener('DOMContentLoaded', function () {
    console.log("Health Check UI Custom JS Loaded");

    // Detaylar penceresindeki belirli bir öğeyi özelleştirme
    let details = document.querySelector('.hc-detail');
    if (details) {
        details.style.backgroundColor = '#f0f8ff';
    }
});