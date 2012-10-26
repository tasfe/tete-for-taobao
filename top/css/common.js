function frameshow(url, id, tabid) {
    //qie tab
    var item = parent.document.getElementsByName("menuli");
    for(i=0;i<4;i++){
        if(tabid == i){
            item[i].ACTIVE;
        }
    }
    //redirect
    parent.document.getElementsByName("mainframe" + id)[0].src = url;
}