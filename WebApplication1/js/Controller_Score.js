app.controller('Score_List', function ($scope, $http) {
    
    $scope.page = 1;
    $scope.SNO = "";
    $scope.CNO = "";
    var getMaxPage = function () {//获得最大的页码数
        $http.get("../asp.net/Server.asmx/GetSCMaxPage?SNO="+$scope.SNO+"&CNO="+$scope.CNO).success(function (result) {
            $scope.maxPage = Math.ceil(result / 10);


        });
    }
    $scope.select = function (page) {//取数据
        $scope.dataset = null;
        $scope.run = true;
        $http.get("../asp.net/Server.asmx/GetSCList?" + "page=" + page+"&SNO="+$scope.SNO+"&CNO="+$scope.CNO).success(function (result) {
            $scope.dataset = result;
            $scope.run = false;
        });
        getMaxPage();
    };
    $scope.delete = function (sno,cno) {//删除按钮

        var data = {
            params: {
                SNO: sno,
                CNO:cno
            }
        };
        $http.get("../asp.net/Server.asmx/DeleteSC", data).success(function (result) {
            if (result == "成功") { alert("成功"); $scope.select($scope.page); }
            else {
                alert("发生错误，请检查数据");
            }
        });
    };
    $scope.select(1);

});
app.controller('Score_Edit', function ($scope, $http, $stateParams, $state) {

    
    


    

    
    console.log($stateParams);
        var data = {
            params: {
                SNO: $stateParams.SNO,
                CNO:$stateParams.CNO
            }
        };
        
        $http.get("../asp.net/Server.asmx/GetSingle_SC", data).success(function (result) {//填充数据

            $scope.SNO = result[0].SNO;
            $scope.CNO = result[0].CNO;
            $scope.SCUScore = result[0].SCUScore;
            $scope.SCEScore = result[0].SCEScore;
            $scope.SCAScore = result[0].SCAScore;
            


        });
        $scope.submit = function () {//有参数时就变为更新了

            var data = {
                params: {
                    SNO: $scope.SNO,
                    CNO: $scope.CNO,
                    SCUScore: $scope.SCUScore,
                    SCEScore: $scope.SCEScore,
                    SCAScore: $scope.SCAScore
                }
            };

            $http.get("../asp.net/Server.asmx/UpdateSC", data).success(function (result) {
                if (result == "成功") { alert("成功"); $state.go('Index.SC.Score_List', $stateParams); }
                else {

                    alert("发生错误，请检查数据");
                }
            });


        };
    }

    


);