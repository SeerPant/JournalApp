window.createChart = (canvasId, chartType, labels, data) => {
    const canvas = document.getElementById(canvasId);
    if (!canvas) return;
       
    if (canvas.chart) {
        canvas.chart.destroy();
    }

    canvas.chart = new Chart(canvas, {
        type: chartType, 
        data: {
            labels: labels,
            datasets: [{
                label: 'My Data',
                data: data
            }]
        },
        options: {
            responsive: true
        }
    });
};