fucntion frameshow(url, id, tabid){
    //qie tab
    var item = parent.document.getElementById("tabCot_product-li-currentBtn-").getElementsByTagName("li");
    for(i=0;i<4;i++){
        if(tabid == i){
            item[i].ACTIVE;
        }
    }
    //redirect
    parent.document.getElementsByName("mainframe" + id).src = url;
}