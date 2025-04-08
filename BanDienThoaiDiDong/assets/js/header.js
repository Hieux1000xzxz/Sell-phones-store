document.addEventListener('DOMContentLoaded', function() {
    const searchInput = document.querySelector('.search-input');
    const searchHistory = document.querySelector('.search-history');
    const historyList = document.querySelector('.history-list');
    const clearHistoryBtn = document.querySelector('.clear-history');

    let searchHistoryItems = [
        'iPhone 14 Pro Max',
        'Samsung Galaxy S23',
        'Xiaomi 13 Pro'
    ];

    // Hiển thị/Ẩn lịch sử tìm kiếm
    searchInput.addEventListener('focus', () => {
        searchHistory.classList.add('show');
    });

    // Ẩn lịch sử khi click ra ngoài
    document.addEventListener('click', (e) => {
        if (!searchInput.contains(e.target) && 
            !searchHistory.contains(e.target)) {
            searchHistory.classList.remove('show');
        }
    });

    // Render lịch sử tìm kiếm
    function renderSearchHistory() {
        historyList.innerHTML = searchHistoryItems.map(item => `
            <li class="history-item">
                <div class="history-item-left">
                    <img src="./assets/icons/clock.svg" alt="clock" class="history-icon"/>
                    <span>${item}</span>
                </div>
                <button class="remove-history" data-item="${item}">&times;</button>
            </li>
        `).join('');

        // Thêm sự kiện cho các nút xóa
        document.querySelectorAll('.remove-history').forEach(btn => {
            btn.addEventListener('click', (e) => {
                e.stopPropagation();
                const item = btn.dataset.item;
                searchHistoryItems = searchHistoryItems.filter(i => i !== item);
                renderSearchHistory();
                // Giữ focus trên input sau khi xóa
                searchInput.focus();
            });
        });

        // Thêm sự kiện click cho các item
        document.querySelectorAll('.history-item').forEach(item => {
            item.addEventListener('click', () => {
                searchInput.value = item.querySelector('span').textContent;
                searchHistory.classList.remove('show');
            });
        });
    }

    // Xóa toàn bộ lịch sử
    clearHistoryBtn.addEventListener('click', (e) => {
        e.stopPropagation(); // Ngăn event bubble lên
        e.preventDefault();
        searchHistoryItems = [];
        renderSearchHistory();
        // Giữ focus trên input sau khi xóa tất cả
        searchInput.focus();
    });

    // Thêm từ khóa mới vào lịch sử khi search
    searchInput.addEventListener('keypress', (e) => {
        if (e.key === 'Enter' && searchInput.value.trim()) {
            const newItem = searchInput.value.trim();
            searchHistoryItems = searchHistoryItems.filter(item => item !== newItem);
            searchHistoryItems.unshift(newItem);
            if (searchHistoryItems.length > 5) {
                searchHistoryItems.pop();
            }
            renderSearchHistory();
        }
    });

    // Khởi tạo lịch sử tìm kiếm
    renderSearchHistory();
}); 