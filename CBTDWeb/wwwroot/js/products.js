var dataTable; 

$(document).ready(function(){
    loadList();
});
function loadList() {
    dataTable = $('#DT_Products').DataTable({
        "ajax": {
            "url": "/api/product",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            //should not be capital
            { "data": "name", "width": "25%" },
            { "data": "listPrice",render: $.fn.dataTable.render.number(',', '.', 2, '$'), "width": "15%" },
            { "data": "category.name", "width": "15%" },
            { "data": "manufacturer.name", "width": "15%" },
            {
                "data": "id", "render": function (data) {
                    return `<div class="text-center">
                            <a href="/Products/Upsert?id=${data}" class="btn btn-success text-white" style="cursor:pointer; width: 100px;"
                                <i class="far fa-edit"></i> Edit </a>
                            <a href="/Products/Delete?id=${data}" class="btn btn-danger text-white" style="cursor:pointer; width: 100px;"
                                <i class="far fa-trash"></i> Delete</a>
                        </div>`;
                }, "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "No data found."
        },
        "width": "100%",
        "order": [[2, "asc"]]
    });
}