function frameshow(url, id, tabid){
    //qie tab
    var item = parent.document.getElementById("tabCot_product-li-currentBtn-").getElementsByTagName("li");
    alert(1);
    for(i=0;i<4;i++){
    alert(item.length);
        if(tabid == i){
            item[i].ACTIVE;
        }
    }
    //redirect
    parent.document.getElementsByName("mainframe" + id).src = url;
}