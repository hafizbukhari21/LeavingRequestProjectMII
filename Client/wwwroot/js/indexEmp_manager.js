﻿google.charts.load('current', { 'packages': ['timeline'] });
google.charts.setOnLoadCallback(drawChart);


$.ajax({
    url: `https://localhost:44302/api/employee/${idEmp}/sisaCuti`
}).done(e => {
    var options = {
        chart: {
            height: 300,
            type: 'radialBar',
        },
        series: [Math.round(parseInt(e.sisaCuti) / 12 * 100)],
        labels: ['Sisa Cuti '+e.sisaCuti+' Hari'],
    }

    var chart = new ApexCharts(document.querySelector("#chartSisaCuti"), options);

    chart.render();
})







function drawChart() {
    console.log("callback " + idEmp)
    $.ajax({
        type: "GET",
        url: `https://localhost:44302/api/leavingrequest/man/${idEmp}/calendar`,

        success: function (response) {
            var container = document.getElementById('timeline');
            var chart = new google.visualization.Timeline(container);
            var dataTable = new google.visualization.DataTable();

            console.log(response)

            let rows = response.map(e => {
                ConvertUTCToDate(e.startDate)
                let colorStatus = ""
                switch (e.status) {
                    case "Menunggu":
                        colorStatus = "#0275d8"
                        break
                    case "Diterima":
                        colorStatus = "#5cb85c"
                        break
                    case "Ditolak":
                        colorStatus = "#d9534f"
                        break
                    case "Revisi":
                        colorStatus = "#f0ad4e"
                        break
                }
                return [
                    e.nameEmployee,
                    e.status,
                    colorStatus,
                    new Date(ConvertUTCToDate(e.startDate)),
                    new Date(ConvertUTCToDate(e.endDate)),


                ]
            })

            console.log(rows)

            dataTable.addColumn({ type: 'string', id: 'Employee' });
            dataTable.addColumn({ type: 'string', id: 'Status' });
            dataTable.addColumn({ type: 'string', role: 'style' });
            dataTable.addColumn({ type: 'date', id: 'Start' });
            dataTable.addColumn({ type: 'date', id: 'End' });

            dataTable.addRows(rows);

            

            chart.draw(dataTable);

        }
    });


}

function ConvertUTCToDate(utc) {
    return moment(utc).format("YYYY-MM-DD").toString()

}
