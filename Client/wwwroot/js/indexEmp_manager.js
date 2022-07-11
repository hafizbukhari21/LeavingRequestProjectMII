drawChart()



$.ajax({
    url: `https://localhost:44302/api/employee/${idEmp}/sisaCuti`
}).done(e => {
    var options = {
        chart: {
            height: 300,
            type: 'radialBar',
            toolbar: {
                show: true,
                offsetX: 0,
                offsetY: 0,
                tools: {
                    download: true,
                    selection: true,
                    zoom: true,
                    zoomin: true,
                    zoomout: true,
                    pan: true,
                    customIcons: []
                },
                export: {
                    csv: {
                        filename: undefined,
                        columnDelimiter: ',',
                        headerCategory: 'category',
                        headerValue: 'value',
                        dateFormatter(timestamp) {
                            return new Date(timestamp).toDateString()
                        }
                    },
                    svg: {
                        filename: undefined,
                    },
                    png: {
                        filename: undefined,
                    }
                },
                autoSelected: 'zoom'
            },
        },
        series: [Math.round(parseInt(e.sisaCuti) / 12 * 100)],
        labels: ['Sisa Cuti '+e.sisaCuti+' Hari'],
    }

    var chart = new ApexCharts(document.querySelector("#chartSisaCuti"), options);

    chart.render();
})







function drawChart() {
    //console.log("callback " + idEmp)
    $.ajax({
        type: "GET",
        url: `https://localhost:44302/api/leavingrequest/man/${idEmp}/calendar`,

        success: function (response) {
            //console.log(response)
            let data = response.filter(e => e.status === "Diterima").map(e => {
                let startDate = Math.floor(new Date(ConvertUTCToDate(e.startDate)).getTime() )
                let endDate = Math.floor(new Date(ConvertUTCToDate(e.endDate)).getTime())
                if (endDate - startDate == 0) {
                    startDate = startDate + 28800000//start jam 8 pagi
                    endDate = endDate + 64800000//sampe jam 6 sore
                }
                return {
                    x: e.nameEmployee,
                    y: [startDate,endDate]
                }
            })
            //console.log(data)
            var options = {
                chart: {
                    height: 300,
                    type: 'rangeBar',
                },
                plotOptions: {
                    bar: {
                        horizontal: true
                    }
                },
                xaxis: {
                    type: 'datetime'
                },
                series: [{
                    data
                }],
            }

            var chart = new ApexCharts(document.querySelector("#timeline"), options);

            chart.render();

        }
    });


}



function ConvertUTCToDate(utc) {
    return moment(utc).format("YYYY-MM-DD").toString()

}
