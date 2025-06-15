let map;
let markers = [];

window.initializeMap = () => {
    map = L.map('map').setView([57.7089, 11.9746], 13);
    
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors'
    }).addTo(map);
};

window.displayLocations = (locations) => {
    markers.forEach(marker => map.removeLayer(marker));
    markers = [];
    
    locations.forEach(location => {
        const icon = getLocationIcon(location.type);
        const marker = L.marker([location.latitude, location.longitude], { icon })
            .bindPopup(`
                <div>
                    <h6>${location.name}</h6>
                    <p><strong>📍 ${location.address}</strong></p>
                    <p>${location.isFree ? '🆓 Gratis' : '💰 Kostnad'}</p>
                    <small>${location.type}</small>
                </div>
            `)
            .addTo(map);
        
        markers.push(marker);
    });
    
    if (markers.length > 0) {
        const group = new L.featureGroup(markers);
        map.fitBounds(group.getBounds().pad(0.1));
    }
};

window.getCurrentLocation = () => {
    return new Promise((resolve, reject) => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    resolve({
                        latitude: position.coords.latitude,
                        longitude: position.coords.longitude
                    });
                },
                (error) => reject(error)
            );
        } else {
            reject(new Error('Geolocation not supported'));
        }
    });
};

window.closeModal = (modalId) => {
    const modal = bootstrap.Modal.getInstance(document.getElementById(modalId));
    if (modal) modal.hide();
};

function getLocationIcon(type) {
    const iconMap = {
        'SecondHand': '🛍',
        'FreeShop': '🎁',
        'RecyclingCenter': '♻️',
        'BikeWorkshop': '🚲',
        'RepairCafe': '🔧'
    };
    
    return L.divIcon({
        html: `<div style="font-size: 24px;">${iconMap[type] || '📍'}</div>`,
        className: 'custom-marker',
        iconSize: [30, 30],
        iconAnchor: [15, 15]
    });
}