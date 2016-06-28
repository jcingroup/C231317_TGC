function getKind(sid) {
    $.post("/Sys_Active/ProductData/getOption", { sid: sid.value })
     .done(function (data) {
         var obj = JSON.parse(data);
         //console.log('hi', obj);

         var x = document.getElementById("sid");
         $("#sid option").remove();
         document.getElementById("sidarea").appendChild(x);
         //新增新的option
         for (i = 0; i < obj.length; i++) {
             var option = document.createElement("option");
             option.text = obj[i].Name;
             option.value = obj[i].ID;
             x.add(option, x[i]);
         };
     });
}

function hi() {
    console.log(document.getElementById("sid").value);
}