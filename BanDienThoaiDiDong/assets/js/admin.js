document.addEventListener('DOMContentLoaded', function () {
    // Vẽ biểu đồ doanh thu
    fetch('AdminDashboard.aspx/GetRevenueChartData', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({})
    })
        .then(response => response.json())
        .then(data => {
            const chartData = JSON.parse(data.d);
            const ctx = document.getElementById('revenueChart').getContext('2d');

            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: chartData.labels,
                    datasets: [{
                        label: 'Doanh thu (VNĐ)',
                        data: chartData.data,
                        borderColor: '#2ecc71',
                        backgroundColor: 'rgba(46, 204, 113, 0.1)',
                        tension: 0.4
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        title: {
                            display: true,
                            text: 'Biểu đồ doanh thu 30 ngày gần nhất'
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: {
                                callback: function (value) {
                                    return new Intl.NumberFormat('vi-VN', {
                                        style: 'currency',
                                        currency: 'VND'
                                    }).format(value);
                                }
                            }
                        }
                    }
                }
            });
        })
        .catch(error => console.error('Error loading chart data:', error));
});