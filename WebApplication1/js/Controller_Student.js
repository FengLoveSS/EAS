app.controller('Student_List', function ($scope, $http) {
    
    $scope.page = 1;
    $scope.SNO = "";
    $scope.SName = "";
    var getMaxPage = function () {//获得最大的页码数
        $http.get("../asp.net/Server.asmx/GetStudentMaxPage?SNO=" + $scope.SNO + "&SName=" + $scope.SName + "&CLNO=" + $scope.Class + "&SYear=" + $scope.SYear).success(function (result) {
            $scope.maxPage = Math.ceil(result / 10);
            
            
        });
    }
    $http.get("../asp.net/Server.asmx/GetAllSYearList").success(function (result) {//填充年份
        result.push({ SYear: "全部"});//添加全部选项
        $scope.dataset_SYear = result;

        $scope.SYear = "全部";

        $scope.select(1);

    });
    $http.get("../asp.net/Server.asmx/GetAllClassList").success(function (result) {//填充班级
        result.push({ CLNO: "全部", CLName: "全部" });//添加全部选项
        $scope.dataset_Class = result;

        $scope.Class = "全部";

        $scope.select(1);

    });
    $scope.select = function (page) {//取数据
        $scope.dataset = null;
        $scope.run = true;
        $http.get("../asp.net/Server.asmx/GetStudentList?" + "page=" + page + "&CLNO=" + $scope.Class + "&SName=" + $scope.SName + "&SNO=" + $scope.SNO + "&SYear=" + $scope.SYear).success(function (result) {
            $scope.dataset = result;
            $scope.run = false;
        });
        getMaxPage();
    };
    $scope.delete = function (sno) {//删除按钮

        var data = {
            params: {
                SNO: sno

            }
        };
        $http.get("../asp.net/Server.asmx/DeleteStudent", data).success(function (result) {
            if (result == "成功") { alert("成功"); $scope.select($scope.page); }
            else {
                alert("发生错误，请检查数据");
            }
        });
    };
    

});
app.controller('Student_Edit', function ($scope, $http, $stateParams, $state) {

    $scope.isUpdate = false;//初始化状态
    $scope.SSex ="男";
    $scope.SBrithday = new Date();
    $http.get("../asp.net/Server.asmx/GetAllClassList").success(function (result) {

        $scope.dataset = result;
        if (result.length != 0) {
            $scope.issCLNO = true;//判断是否存在班级可以选，此时为存在
            if ($scope.Submit == "新建")
            $scope.Class = result[0].CLNO;
        } else {
            $scope.issCLNO = false;
        }


    });

    
    if ($stateParams.SNO == "") {

        $scope.Submit = "新建";
        $scope.submit = function () {//新建新班级

            var data = {
                params: {
                    SNO: $scope.SNO,
                    SName: $scope.SName,
                    SSex: $scope.SSex,
                    SBrithday: $scope.SBrithday,
                    SGrade: $scope.SGrade,
                    SYear: $scope.SYear,
                    CLNO:$scope.Class
                }
            };
            
            $http.get("../asp.net/Server.asmx/NewStudent", data).success(function (result) {
                if (result == "成功") { alert("成功"); $state.go('Index.Manage.Student_List', $stateParams); }
                else {
                    alert("发生错误，请检查数据");
                }
            });


        };



    }
    else {

        $scope.isUpdate = true;

        $scope.Submit = "修改";
        var data = {
            params: {
                SNO: $stateParams.SNO
            }
        };

        $http.get("../asp.net/Server.asmx/GetSingle_Student", data).success(function (result) {//填充数据

            $scope.SNO = result[0].SNO;
            $scope.SName = result[0].SName;
            $scope.SSex = result[0].SSex;
            $scope.SBrithday = result[0].SBrithday;
            $scope.SGrade = result[0].SGrade;
            $scope.SYear = result[0].SYear;
            $scope.Class = result[0].CLNO;
            console.log(result[0].CLNO);

        });
        $scope.submit = function () {//有参数时就变为更新了

            var data = {
                params: {
                    SNO: $scope.SNO,
                    SName: $scope.SName,
                    SSex: $scope.SSex,
                    SBrithday: $scope.SBrithday,
                    SGrade: $scope.SGrade,
                    SYear: $scope.SYear,
                    CLNO: $scope.Class
                }
            };

            $http.get("../asp.net/Server.asmx/UpdateStudent", data).success(function (result) {
                if (result == "成功") { alert("成功"); $state.go('Index.Manage.Student_List', $stateParams); }
                else {

                    alert("发生错误，请检查数据");
                }
            });


        };
    }

    $scope.find = function () {//查找是否存在该班级号

        var data = {
            params: {
                SNO: $scope.SNO

            }
        };

        $http.get("../asp.net/Server.asmx/FindStudent_SNO", data).success(function (result) {
            if (result == "true") {
                $scope.isSNO = true;
            }
            else if (result == "false") {
                $scope.isSNO = false;
            }


        });
    }


});