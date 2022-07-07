
$(document).ready(function () {

    // get emp data
    $.ajax({
        type: "GET",
        url: "https://localhost:44302/api/employee",
        dataType: "json",
    }).done((result) => {

        let man1 = [...result.filter(e => e.roleName == "Manager")].length
        let emp1 = [...result.filter(e => e.roleName == "Employee")].length
        let total1 = [...result.filter(e => e.roleName != "Admin")].length

        let male = [...result.filter(g => g.gender == "Male" && g.roleName != "Admin")].length
        let female = [...result.filter(g => g.gender == "Female" && g.roleName != "Admin")].length

        console.log("man " + man1)
        console.log("emp " + emp1)
        console.log("total man + emp " + total1)

        console.log("Male " + male)
        console.log("Female " + female)

        $("#total_emp").html(total1);
        $("#man").html(man1);
        $("#emp").html(emp1);

    // chart from emp data
        var options = {
            series: [male, female],
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
})

$(document).ready(function () {
    // get emp data
    $.ajax({
        type: "GET",
        url: "https://localhost:44302/api/employee",
        dataType: "json",
    }).done((result) => {

        let man1 = [...result.filter(e => e.roleName == "Manager")].length
        let emp1 = [...result.filter(e => e.roleName == "Employee")].length
        let total1 = [...result.filter(e => e.roleName != "Admin")].length

        let male = [...result.filter(g => g.gender == "Male" && g.roleName != "Admin")].length
        let female = [...result.filter(g => g.gender == "Female" && g.roleName != "Admin")].length

        console.log("man " + man1)
        console.log("emp " + emp1)
        console.log("total man + emp " + total1)

        console.log("Male " + male)
        console.log("Female " + female)

        $("#total_emp").html(total1);
        $("#man").html(man1);
        $("#emp").html(emp1);


            var options = {
                chart: {
                    type: 'bar'
                },
                series: [{
                    name: 'Total',
                    data: [man1, emp1]
                }],
                xaxis: {
                    categories: ["Manager", "Employee"]
                },
                legend: {
                    show: true,
                    position: 'top'
                }
            }
            var chart = new ApexCharts(document.querySelector("#myBarChart"), options);
            chart.render();
        })
    })