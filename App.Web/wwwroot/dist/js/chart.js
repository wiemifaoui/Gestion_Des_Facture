
$(function () {

    /*
    * BAR CHART
    * ---------
    */

    var bar_data = {
        data: [[1, 10], [2, 8], [3, 4], [4, 13], [5, 17], [6, 9]],
        bars: { show: true }
    }
    $.plot('#bar-chart', [bar_data], {
        grid: {
            borderWidth: 1,
            borderColor: '#f3f3f3',
            tickColor: '#f3f3f3'
        },
        series: {
            bars: {
                show: true, barWidth: 0.5, align: 'center',
            },
        },
        colors: ['#3c8dbc'],
        xaxis: {
            ticks: [[1, 'January'], [2, 'February'], [3, 'March'], [4, 'April'], [5, 'May'], [6, 'June']]
        }
    })
    /* END BAR CHART */
})