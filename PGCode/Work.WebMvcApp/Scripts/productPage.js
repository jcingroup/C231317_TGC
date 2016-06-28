function changePage1(sel,sid) {
    document.location.href = 'Products_second?page='+sel.value+'&sid='+sid;
}
function changePage2(sel, sid) {
    document.location.href = 'Products_second2?page=' + sel.value + '&sid=' + sid;
}
function NewChangePage1(sel, sid) {
    document.location.href = 'Products_new?page=' + sel.value + '&sid=' + sid;
}
function NewChangePage2(sel, sid) {
    document.location.href = 'Products_new2?page=' + sel.value + '&sid=' + sid;
}