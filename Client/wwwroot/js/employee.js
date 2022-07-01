$(document).ready(function () {
    $('#dataTbl').DataTable({
        "ajax": {
            "url": "https://localhost:44302/api/employee",
            "dataType": "json",
            "dataSrc": "",
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
                text: '<i class="fa fa-file-excel"> </i>',
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
                <button title="Add new data" type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#insertModal">
                    <i class="fa fa-plus"></i>
                </button>`
            }
        ],
        initComplete: function () {
            var btns = $('.dt-button');
            //btns.addClass('btn btn-primary btn-sm');
            btns.removeClass('dt-button');
        },
        "columns": [
            {
                "data": null,
                "sortable": true,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
            },
            {
                "data": "name"
            },
            {
                "data": "email"
            },
            {
                "data": "phoneNumber"
            },
            {
                "data": "roleName"
            },
            {
                "data": "namaDivisi"
            },
            {
                data: "employee_id",
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

/*$(document).ready(function () {
    $('#dataTbl').DataTable({
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
                text: '<i class="fas fa-file-excel"> </i>',
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
                <button title="Add new data" type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#insertModal">
                    <i class="fa fa-plus"></i>
                </button>`
            }
        ],
        initComplete: function () {
            var btns = $('.dt-button');
            //btns.addClass('btn btn-primary btn-sm');
            btns.removeClass('dt-button');
        },
    });
});*/



/*function insertEmployee() {
    const FIELD_REQUIRED = "This field is required.";
    const EMAIL_INVALID = "Please enter a valid email address format.";

    let emailValid = validateEmail(document.getElementById("validationCustomEmail").value, FIELD_REQUIRED, EMAIL_INVALID);
    let emailDuplicate = validateDuplicateData(document.getElementById("validationCustomEmail").value, "email");
    let phoneDuplicate = validateDuplicateData(document.getElementById("validationCustom03").value, "phone");
    if (emailValid && !emailDuplicate && !phoneDuplicate) {
        //Set Employee Data
        let firstName = document.getElementById("validationCustom01").value;
        let lastName = document.getElementById("validationCustom02").value;
        let email = document.getElementById("validationCustomEmail").value;
        let phone = document.getElementById("validationCustom03").value;
        let gender = document.getElementById("inputGender").value;
        let birthDate = document.getElementById("validationCustom04").value;
        let degree = document.getElementById("inputDegree").value;
        let gpa = document.getElementById("validationCustom05").value;
        let university = document.getElementById("inputUniversity").value;

        let res = registerData(firstName, lastName, email, phone, gender, birthDate, degree, gpa, university);

        table.ajax.reload();

        $('#modalForm').modal('toggle');
    }
}*/


$("#insertEmployee").submit(function (e) {
    e.preventDefault();
    var obj = new Object(); 
    obj.name = $("#CName").val();
    obj.gender = $("#CGender").val();
    obj.email = $("#CEmail").val();
    obj.password = $("#CPassword").val();
    obj.phoneNumber = parseInt($("#CPhoneNumber").val());
    obj.role_Id = $("#CRoleId").val();
    obj.manager_Id = $("#CManagerId").val();
    obj.divisi_Id = $("#CDivisiId").val();
    console.log(obj);
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        url: "https://localhost:44302/api/employee",
        type: "POST",
        contentType: 'application/json',
        //data: obj,
        data: JSON.stringify(obj), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
    }).done((result) => {
        console.log(result)
        $('#dataTbl').DataTable().ajax.reload();
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: result.message,
            footer: '<a href="">Why do I have this issue?</a>'
        })
        formReset();
    }).fail((error) => {
        switch (error.errorType) {
            case 1:
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: error.message,
                })
                break;
            case 2:
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: error.message,
                })
                break;
            default:
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: "Mohon ini semua field dengan benar !!",
                })
                break;
        }
        console.log(error)
    })
})

function formReset() {
    document.getElementById("insertEmployee").reset();
    $('#btnInput').prop("disabled", false);
}