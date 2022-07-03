const idEmp = $("#login-employee-id").val()


$(document).ready(function () {

    let man = {
        "manager_id": "Employee0007"
    }

    $('#tblMan').DataTable({
        ajax: {
            url: "https://localhost:44302/api/leavingrequest/man",
            type: "POST",
            "contentType": "application/json",
            "data": JSON.stringify(man)
        },
        dom: 'lBfrtip',
        buttons: [
            //'copy', 'csv', 'excel', 'pdf', 'print'
            {
                extend: 'copy',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-copy"> </i>',
                titleAttr: 'Copy to clipboard'
            },
            {
                extend: 'excel',
                className: 'btn btn-success btn-sm',
                text: '<i class="far fa-file-excel"> </i>',
                titleAttr: 'Download to excel'
            },
            {
                extend: 'pdf',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-file-pdf"> </i>',
                titleAttr: 'Download to pdf'
            },
            {
                extend: 'print',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-print"> </i>',
                titleAttr: 'Print this table'
            },
            {
                html: `<!-- Button trigger modal -->
                <button title="Add new data" type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#exampleModal">
                    <i class="fa fa-plus"></i>
                </button>`
            }
        ],
        initComplete: function () {
            var btns = $('.dt-button');
            //btns.addClass('btn btn-primary btn-sm');
            btns.removeClass('dt-button');
        },
        columns: [
            {
                "data": null,
                "sortable": true,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
            },
            {
                "data": "employeeName"
            },
            {
                "data": "categoryName"
            },
            {
                "data": "approvalStatus"
            },
            {
                data: "request_id",
                render: function (data, type, row, meta) {
                    return `<div class="btn-group text-center">
                                <button type="button" class="btn btn-sm btn-warning" title="Delete" onClick="employeeDetail('Employee/Get/${data}')" data-toggle="modal" data-target="#updateModal">
                                <i class="fas fa-edit"></i>
                                </button>
                                <button type="button" class="btn btn-sm btn-danger" title="Delete" onClick="employeeDelete('Employee/Delete/${data}')" >
                                <i class="fas fa-trash"></i>
                                </button>
                            </div>`
                }
            }
        ]
    });
});
