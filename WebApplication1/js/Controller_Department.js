
//getϵ���б�
app.controller('Department_List', function ($scope, $http) {
    $scope.page = 1;
    $scope.DNO = "";
    $scope.DName = "";
    var getMaxPage = function () {//�������ҳ����
        $http.get("../asp.net/Server.asmx/GetDepartmentMaxPage?DNO="+$scope.DNO+"&DName="+$scope.DName).success(function (result) {
            $scope.maxPage = Math.ceil(result / 10);
        });
    }
    $scope.select = function (page) {//ȡ����
        $scope.dataset = null;
        $scope.run = true;
        $http.get("../asp.net/Server.asmx/GetDepartmentList?" + "page=" + page + "&DName=" + $scope.DName + "&DNO=" + $scope.DNO).success(function (result) {
            $scope.run = false;
            
            $scope.dataset = result;
        });
        getMaxPage();
        
    };
    $scope.delete = function (dno) {//ɾ����ť
        
        var data = {
            params: {
                DNO: dno

            }
        };
        $http.get("../asp.net/Server.asmx/DeleteDepartment", data).success(function (result) {
            if (result == "�ɹ�") { alert("�ɹ�"); $scope.select(1); }
            else {
                
                alert("����������������");
            }
        });
    };
    $scope.select(1);
    
});
app.controller('Department_Edit', function ($scope,$http,$stateParams,$state) {
    $scope.isUpdate = false;
    if ($stateParams.DNO == "") {//û�в���ʱΪ�½���Ϣ
        $scope.Submit = "�½�";
        $scope.submit = function () {

            var data = {
                params: {
                    DNO: $scope.DNO,
                    DName: $scope.DName,
                    DIntroduction: $scope.DIntroduction
                }
            };

            $http.get("../asp.net/Server.asmx/NewDepartment", data).success(function (result) {
                if (result == "�ɹ�") { alert("�ɹ�"); $state.go('Index.Manage.Department_List', $stateParams); }
                else {

                    alert("����������������");
                }
            });


        };
        
        
    }
    else {
        $scope.isUpdate = true;
        $scope.Submit = "�޸�";
        var data = {
            params: {
                DNO: $stateParams.DNO
            }
        };

        $http.get("../asp.net/Server.asmx/GetSingle_Department", data).success(function (result) {
            
            $scope.DNO = result[0].DNO;
            $scope.DName = result[0].DName;
            $scope.DIntroduction = result[0].DIntroduction;
        });
        $scope.submit = function () {//�в���ʱ�ͱ�Ϊ������

            var data = {
                params: {
                    DNO: $scope.DNO,
                    DName: $scope.DName,
                    DIntroduction: $scope.DIntroduction
                }
            };

            $http.get("../asp.net/Server.asmx/UpdateDepartment", data).success(function (result) {
                if (result == "�ɹ�") { alert("�ɹ�"); $state.go('Index.Manage.Department_List', $stateParams); }
                else {

                    alert("����������������");
                }
            });


        };
    }
    
    $scope.find = function () {//����Ƿ����DNO
        var data = {
            params: {
                DNO: $scope.DNO
                
            }
        };

        $http.get("../asp.net/Server.asmx/FindDepartment_DNO", data).success(function (result) {
            if (result == "true")
            {
                $scope.isDNO = true;
            }
            else if(result == "false")
            {
                $scope.isDNO = false;
            }
            
        });
    }


});
