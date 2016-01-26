app.controller('Course_List', function ($scope, $http) {
    $scope.page = 1;
    $scope.CNO = "";
    $scope.CName = ""; 
    $scope.TName = "";
    var getMaxPage = function () {//获得最大的页码数
        $http.get("../asp.net/Server.asmx/GetCourseMaxPage?CNO=" + $scope.CNO + "&CName=" + $scope.CName + "&TName=" + $scope.TName+"&DNO="+$scope.Department).success(function (result) {
            $scope.maxPage = Math.ceil(result / 10);
        });
    }
    $http.get("../asp.net/Server.asmx/GetAllDepartmentList").success(function (result) {
        result.push({ DNO: "全部", DName: "全部" });//添加全部选项
        $scope.dataset_Department = result;

        $scope.Department = "全部";

        $scope.select(1);

    });
    $scope.select = function (page) {//取数据
        $scope.dataset = null;
        $scope.run = true;
        $http.get("../asp.net/Server.asmx/GetCourseList?" + "page=" + page + "&DNO=" + $scope.Department + "&TName=" + $scope.TName + "&CNO=" + $scope.CNO+"&CName="+$scope.CName).success(function (result) {
            $scope.dataset = result;
            $scope.run = false;
        });
        getMaxPage();
    };
    $scope.delete = function (cno) {//删除按钮

        var data = {
            params: {
                CNO: cno

            }
        };
        $http.get("../asp.net/Server.asmx/DeleteCourse", data).success(function (result) {
            if (result == "成功") { alert("成功"); $scope.select($scope.page); }
            else {
                alert("发生错误，请检查数据");
            }
        });
    };
    

});
app.controller('Course_Edit', function ($scope, $http, $stateParams, $state) {

    $scope.isUpdate = false;//初始化状态
    $http.get("../asp.net/Server.asmx/GetAllTeacherList").success(function (result) {
        $scope.CSemester = "1";
        $scope.dataset = result;
        if (result.length != 0) {
            $scope.issTNO = true;//判断是否存在教师可以选，此时为存在
            if ($scope.Submit == "新建")
            $scope.Teacher = result[0].TNO;
        } else {
            $scope.issTNO = false;
        }


    });


    if ($stateParams.CNO == "") {

        $scope.Submit = "新建";
        $scope.submit = function () {//新建新班级

            var data = {
                params: {
                    CNO: $scope.CNO,
                    CName: $scope.CName,
                    TNO: $scope.Teacher,
                    CHours: $scope.CHours,
                    CCredit: $scope.CCredit,
                    CTime: $scope.CTime,
                    CSemester: $scope.CSemester,
                    CPlace: $scope.CPlace,
                    CExamTime: $scope.CExamTime
                }
            };

            $http.get("../asp.net/Server.asmx/NewCourse", data).success(function (result) {
                if (result == "成功") { alert("成功"); $state.go('Index.Manage.Course_List', $stateParams); }
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
                CNO: $stateParams.CNO
            }
        };

        $http.get("../asp.net/Server.asmx/GetSingle_Course", data).success(function (result) {//填充数据

            $scope.CNO = result[0].CNO;
            $scope.CName = result[0].CName;
            $scope.Teacher = result[0].TNO;
            $scope.CHours = result[0].CHours;
            $scope.CCredit = result[0].CCredit;
            $scope.CTime = result[0].CTime;
            $scope.CSemester = result[0].CSemester;
            $scope.CPlace = result[0].CPlace;
            $scope.CExamTime = result[0].CExamTime;

        });
        $scope.submit = function () {//有参数时就变为更新了

            var data = {
                params: {
                    CNO: $scope.CNO,
                    CName: $scope.CName,
                    TNO: $scope.Teacher,
                    CHours: $scope.CHours,
                    CCredit: $scope.CCredit,
                    CTime: $scope.CTime,
                    CSemester: $scope.CSemester,
                    CPlace: $scope.CPlace,
                    CExamTime: $scope.CExamTime
                }
            };

            $http.get("../asp.net/Server.asmx/UpdateCourse", data).success(function (result) {
                if (result == "成功") { alert("成功"); $state.go('Index.Manage.Course_List', $stateParams); }
                else {

                    alert("发生错误，请检查数据");
                }
            });


        };
    }

    $scope.find = function () {//查找是否存在该班级号

        var data = {
            params: {
                CNO: $scope.CNO

            }
        };

        $http.get("../asp.net/Server.asmx/FindCourse_CNO", data).success(function (result) {
            if (result == "true") {
                $scope.isCNO = true;
            }
            else if (result == "false") {
                $scope.isCNO = false;
            }


        });
    }


});