//id表示需加入分页展示代码的容器标签"#Id",props表示带有总页数、当前页数的Json值
function PageCreate(id, props) {
    if (id && props) {
        this.id = id;
        this.pageCount = parseInt(props.pageCount),
        this.currentPage = parseInt(props.currentPage),
        this.pageDom = $(id);
        this.clickPage = null;
    } else {
        console.log("请传入正确参数");
        return false;
    }
    //若是总页数小于每页按钮数
    if (this.pageCount <= 7) {
        this.pageDom.html("");
        $('<li class="page-change"><a href="javascript:void(null)" aria-label="First page" pagenum="' + (this.currentPage == 1 ? 1 : (this.currentPage - 1)) + '">上一页</a></li>').appendTo(this.pageDom);
        for (var i = 1; i <= this.pageCount; i++) {
            $('<li class="pageNum"><a href="javascript:void(null)"  pagenum="' + i + '" >' + i + '</a></li>').appendTo(this.pageDom);
            /*if (i == this.currentPage) {
                this.pageDom.children("li.pageNum").last().addClass('pro-list-current');
            }*/
        }
        $('<li class="page-change"><a href="javascript:void(null)" aria-label="Last page" pagenum="' + (this.currentPage == this.pageCount ? this.pageCount : (this.currentPage + 1)) + '">下一页</a></li>').appendTo(this.pageDom);
    } else {
        this.pageDom.html("");
        $('<li class="page-change"><a href="javascript:void(null)" aria-label="First page" pagenum="' + (this.currentPage == 1 ? 1 : (this.currentPage - 1)) + '">上一页</a></li>').appendTo(this.pageDom);
        if (this.currentPage == 1 || this.currentPage == this.pageCount) {
            $('<li class="pageNum"><a href="javascript:void(null)"  pagenum="1" >1</a></li class="pageNum"><li><a href="javascript:void(null)"  pagenum="2" >2</a></li>').appendTo(this.pageDom);
            $('<li><a href="javascript:void(null)">...</a></li>').appendTo(this.pageDom);
            $('<li class="pageNum"><a href="javascript:void(null)"  pagenum="' + (this.pageCount - 1) + '" >' + (this.pageCount - 1) + '</a></li><li class="pageNum"><a href="javascript:void(null)"  pagenum="' + this.pageCount + '" >' + this.pageCount + '</a></li>').appendTo(this.pageDom);
        } else if (this.currentPage == 2 || this.currentPage == 3 || this.currentPage == 4) {
            $('<li class="pageNum"><a href="javascript:void(null)"  pagenum="1" >1</a></li><li class="pageNum"><a href="javascript:void(null)"  pagenum="2" >2</a></li>').appendTo(this.pageDom);
            for (var i = 1; i < this.currentPage; i++) {
                $('<li class="pageNum"><a href="javascript:void(null)"  pagenum="' + (i + this.currentPage) + '" >' + (i + this.currentPage) + '</a></li>').appendTo(this.pageDom);
            }
            $('<li><a href="javascript:void(null)">...</a></li>').appendTo(this.pageDom);
            $('<li class="pageNum"><a href="javascript:void(null)"  pagenum="' + (this.pageCount - 1) + '" >' + (this.pageCount - 1) + '</a></li><li class="pageNum"><a href="javascript:void(null)"  pagenum="' + this.pageCount + '" >' + this.pageCount + '</a></li>').appendTo(this.pageDom);
        } else if (this.currentPage == this.pageCount - 1 || this.currentPage == this.pageCount - 2 || this.currentPage == this.pageCount - 3) {
            $('<li class="pageNum"><a href="javascript:void(null)"  pagenum="1" >1</a></li><li class="pageNum"><a href="javascript:void(null)"  pagenum="2" >2</a></li>').appendTo(this.pageDom);
            $('<li><a href="javascript:void(null)">...</a></li>').appendTo(this.pageDom);
            for (var i = this.pageCount-1; i >= this.currentPage; i--) {
                $('<li class="pageNum"><a href="javascript:void(null)"  pagenum="' + (this.currentPage - (this.pageCount - i)) + '" >' + (this.currentPage - (this.pageCount - i)) + '</a></li>').appendTo(this.pageDom);
            }
            $('<li class="pageNum"><a href="javascript:void(null)"  pagenum="' + (this.pageCount - 1) + '" >' + (this.pageCount - 1) + '</a></li><li class="pageNum"><a href="javascript:void(null)"  pagenum="' + this.pageCount + '" >' + this.pageCount + '</a></li>').appendTo(this.pageDom);
        } else{
            $('<li class="pageNum"><a href="javascript:void(null)"  pagenum="1" >1</a></li><li class="pageNum"><a href="javascript:void(null)"  pagenum="2" >2</a></li>').appendTo(this.pageDom);
            $('<li><a href="javascript:void(null)">...</a></li>').appendTo(this.pageDom);
            $('<li class="pageNum"><a href="javascript:void(null)"  pagenum="' + (this.currentPage - 1) + '" >' + (this.currentPage - 1) + '</a></li><li class="pageNum"><a href="javascript:void(null)"  pagenum="' + this.currentPage + '" >' + this.currentPage + '</a></li><li class="pageNum"><a href="javascript:void(null)"  pagenum="' + (this.currentPage + 1) + '" >' + (this.currentPage + 1) + '</a></li>').appendTo(this.pageDom);
            $('<li><a href="javascript:void(null)">...</a></li>').appendTo(this.pageDom);
            $('<li class="pageNum"><a href="javascript:void(null)"  pagenum="' + (this.pageCount - 1) + '" >' + (this.pageCount - 1) + '</a></li><li class="pageNum"><a href="javascript:void(null)"  pagenum="' + this.pageCount + '" >' + this.pageCount + '</a></li>').appendTo(this.pageDom);
        } 
        $('<li class="page-change"><a href="javascript:void(null)" aria-label="Last page" pagenum="' + (this.currentPage == this.pageCount ? this.pageCount : (this.currentPage + 1)) + '">下一页</a></li>').appendTo(this.pageDom);
    }
    eachul(this.id, this.currentPage);
}
function eachul(id, currentPage) {
    $("" + id + " li.pageNum").removeClass('pro-list-current');
    $("" + id + " li.pageNum").each(function (index) {
        if ($(this).find('a').attr("pagenum") == currentPage) {
            $(this).addClass('pro-list-current');
        }
    })
}
PageCreate.prototype.afterClick = function (func) {
    this.pageDom.children('li.pageNum').off("click").on("click", function (event) {
        if ($(this).hasClass('pro-list-current') != true) {
            var clickPage = parseInt($(this).children('a').attr("pagenum"));
            //console.log("pageNum = "+clickPage);
            //翻页按钮点击后触发的回调函数
            func(clickPage);
        } else {
            return false;
        }
    });
    this.pageDom.children('li.page-change').off("click").on("click", function (event) {
        var clickPage = parseInt($(this).children('a').attr("pagenum"));
        func(clickPage);
    });
}