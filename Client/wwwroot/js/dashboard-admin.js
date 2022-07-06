$(document).ready(function () {

    $.ajax({
        type: "GET",
        url: "https://localhost:44302/api/employee",
        dataType: "json",
    }).done((result) => {
        let m = 0
        let fm = 0

        $.each(result, function (key, val) {
            if (val.gender == 0) {
                m += 1
            } else if (val.gender == 1) {
                fm += 1
            }
        })
        let total = m + fm
        console.log("male : ", m)
        console.log("female : ", fm)
        console.log("Total : ", total)


        var options = {
            series: [m, fm],
            labels: ['Male', 'Female'],
            chart: {
                type: 'donut',
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
                        reset: true | '<img src="/static/icons/reset.png" width="20">',
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
            plotOptions: {
                pie: {
                    donut: {
                        labels: {
                            show: true,
                            name: {
                                show: true
                            },
                            value: {
                                show: true
                            },
                            total: {
                                show: true,
                                color: 'black',
                                label: 'Total',
                            }
                        }
                    }
                }
            },
        }

        var chart = new ApexCharts(document.querySelector("#myDonutChart"), options);

        chart.render();
    })
});