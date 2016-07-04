function getKind(sid) {
    var is_second = $("input[name='IsSecond']:checked").val();
    $.post("/Sys_Active/ProductData/getOption", { sid: sid, is_second: is_second })
     .done(function (data) {
         var obj = JSON.parse(data);
         //console.log('hi', obj);

         var x = document.getElementById("sid");
         $("#sid option").remove();
         document.getElementById("sidarea").appendChild(x);
         //新增新的option
         for (i = 0; i < obj.length; i++) {
             var option = document.createElement("option");
             option.text = obj[i].Text;
             option.value = obj[i].Value;
             x.add(option, x[i]);
         };
     });
}

function hi() {
    console.log(document.getElementById("sid").value);
}