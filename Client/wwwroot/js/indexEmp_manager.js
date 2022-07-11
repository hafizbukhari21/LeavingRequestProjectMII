google.charts.load('current', { 'packages': ['timeline'] });
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
                    case "Cancel":
                        colorStatus = "#292b2c"
                        break
                }
                return [
                    e.nameEmployee,
                    e.status,
                    colorStatus,
                    new Date(ConvertUTCToDate(e.startDate)),
                    new Date(ConvertUTCToDate(e.endDate)),
                    `<h1>dfdf adasdsad as dsad sad sad </h1>`,


                ]
            })

            console.log(rows)

            dataTable.addColumn({ type: 'string', id: 'Employee' });
           
            dataTable.addColumn({ type: 'string', id: 'Status' });
            dataTable.addColumn({ type: 'string', role: 'style' });
            dataTable.addColumn({ type: 'date', id: 'Start' });
            dataTable.addColumn({ type: 'date', id: 'End' });
            dataTable.addColumn({ type: 'string', role: 'tooltip', p: { html: true } });

            dataTable.addRows(rows);


            var options = {
                tooltip: { isHtml: true }
            }

            chart.draw(dataTable);


            function myHandler(e) {
                let startDate = ConvertUTCToDate(dataTable.getValue(e.row, 3))
                let endDate = ConvertUTCToDate(dataTable.getValue(e.row, 4))
                let duration = ((new Date(endDate) - new Date(startDate)) / (1000 * 3600 * 24))+1
               
                if (e.row != null) {
                    
                    $(".google-visualization-tooltip").html(`
                        <div class="card" style="width: 18rem;">
                              <div class="card-body">
                                <h5 class="card-title">${dataTable.getValue(e.row, 1)}</h5>
                                <p class="card-text">Tanggal Cuti dari ${startDate} s.d. ${endDate}</p>
                                <p class="card-text">Durasi ${duration} Hari</p>
                                
                                </div>
                            </div>

                    `).css({ width: "auto", height: "auto" });;
                }
            }

            google.visualization.events.addListener(chart, 'onmouseover', myHandler);

            function customTooltip() {
                return `<h1>dfdf adasdsad as dsad sad sad </h1>`
            }

        }
    });


}



function ConvertUTCToDate(utc) {
    return moment(utc).format("YYYY-MM-DD").toString()

}
