﻿
angular.module('app.controllers').controller('EmissoraLancDespesaController',
['$scope', '$rootScope', '$state', 'Api', 'ngSelects', 'ngHistoricoFiltro',
function ($scope, $rootScope, $state, Api, ngSelects, ngHistoricoFiltro )
{
	$rootScope.exibirMenu = true;
	$scope.loading = false;

	init();

	function init()
    {
        $scope.campos =
            {
                valor: '0,00',
                codigo: '',
                nomeAssociado: '',
                tipo: '',
                selectTipoAutorizacao: ngSelects.obterConfiguracao(Api.TipoAutorizacaoCombo, {})
            };

        $scope.itensporpagina = 15;        
	}

    $scope.$watch('campos.tipo', function (newState, oldState) {
        if (newState !== oldState)
        {
            $scope.list = undefined;
            $scope.campos.selecionado = undefined;

            if ($scope.paginadorMaterial != undefined) $scope.paginadorMaterial.reiniciar();
            if ($scope.paginadorDiaria != undefined) $scope.paginadorDiaria.reiniciar();
            if ($scope.paginadorMed != undefined) $scope.paginadorMed.reiniciar();
            if ($scope.paginadorNaoMed != undefined) $scope.paginadorNaoMed.reiniciar();
            if ($scope.paginadorOPME != undefined) $scope.paginadorOPME.reiniciar();
            if ($scope.paginadorPacote != undefined) $scope.paginadorPacote.reiniciar();

            ngHistoricoFiltro.filtro.paginaAtual = 0;
        }            
    });

    $scope.buscaCartao = function () {
        $scope.loading = true;

        var opcoes = { skip: 0, take: 1, matricula: $scope.campos.matricula };

        Api.Associado.listPage(opcoes, function (data) {
            if (data.results.length > 0) {
                $scope.campos.nomeAssociado = data.results[0].stName;
            }
            $scope.loading = false;
        });
    }

    $scope.show = function (mdl) {
        $scope.campos.selecionado = mdl;
    }
    
    $scope.cancelar = function (mdl) {
        $scope.campos.selecionado = undefined;
    }

    // ------------------
    // DIARIA
    // ------------------

	$scope.searchDiaria = function ()
    {
        $scope.campos.selecionado = undefined;
        $scope.loadDiaria(0, $scope.itensporpagina);
        if ($scope.paginadorDiaria != undefined)
            $scope.paginadorDiaria.reiniciar();

        $scope.paginaAtual = 1;
	}
    
	$scope.loadDiaria = function (skip, take)
	{
		$scope.loading = true;

        var opcoes = {
            skip: skip,
            take: take,
            codigo: $scope.campos.codigo,
            desc: $scope.campos.desc,
        };

		Api.PrecoDiaria.listPage(opcoes, function (data)
		{
			$scope.list = data.results;
			$scope.total = data.count;
			$scope.loading = false;
        },
        function (response) {
            $scope.loading = false;
        });
    }

    // ------------------
    // MATERIAL
    // ------------------

    $scope.searchMaterial = function () {
        $scope.campos.selecionado = undefined;
        $scope.loadMaterial(0, $scope.itensporpagina);
        if ($scope.paginadorMaterial != undefined)
            $scope.paginadorMaterial.reiniciar();
    }

    $scope.loadMaterial = function (skip, take) {
        $scope.loading = true;

        var opcoes = {
            skip: skip,
            take: take,
            codigo: $scope.campos.codigo,
            desc: $scope.campos.desc,
        };

        Api.PrecoMaterial.listPage(opcoes, function (data) {
            $scope.list = data.results;
            $scope.total = data.count;
            $scope.loading = false;
        },
            function (response) {
                $scope.loading = false;
            });
    }

    // ------------------
    // MEDICAMENTOS
    // ------------------

    $scope.searchMed = function () {
        $scope.campos.selecionado = undefined;
        $scope.loadMed(0, $scope.itensporpagina);
        if ($scope.paginadorMed != undefined)
            $scope.paginadorMed.reiniciar();
    }

    $scope.loadMed = function (skip, take) {
        $scope.loading = true;

        var opcoes = {
            skip: skip,
            take: take,
            codigo: $scope.campos.codigo,
            desc: $scope.campos.desc,
        };

        Api.PrecoMedicamento.listPage(opcoes, function (data) {
            $scope.list = data.results;
            $scope.total = data.count;
            $scope.loading = false;
        },
            function (response) {
                $scope.loading = false;
            });
    }

    // ------------------
    // Nao medicos
    // ------------------

    $scope.searchNaoMed = function () {
        $scope.campos.selecionado = undefined;
        $scope.loadNaoMed(0, $scope.itensporpagina);
        if ($scope.paginadorNaoMed != undefined)
            $scope.paginadorNaoMed.reiniciar();
    }

    $scope.loadNaoMed = function (skip, take) {
        $scope.loading = true;

        var opcoes = {
            skip: skip,
            take: take,
            codigo: $scope.campos.codigo,
            desc: $scope.campos.desc,
        };

        Api.PrecoNaoMedico.listPage(opcoes, function (data) {
            $scope.list = data.results;
            $scope.total = data.count;
            $scope.loading = false;
        },
            function (response) {
                $scope.loading = false;
            });
    }

    // ------------------
    // OPME
    // ------------------

    $scope.searchOPME = function () {
        $scope.campos.selecionado = undefined;
        $scope.loadOPME(0, $scope.itensporpagina);
        if ($scope.paginadorOPME != undefined)
            $scope.paginadorOPME.reiniciar();
    }

    $scope.loadOPME = function (skip, take) {
        $scope.loading = true;

        var opcoes = {
            skip: skip,
            take: take,
            codigo: $scope.campos.codigo,
            desc: $scope.campos.desc,
        };

        Api.PrecoOPME.listPage(opcoes, function (data) {
            $scope.list = data.results;
            $scope.total = data.count;
            $scope.loading = false;
        },
        function (response) {
            $scope.loading = false;
        });
    }

    // --------------
    // PACOTE
    // --------------

    $scope.searchPacote = function () {
        $scope.campos.selecionado = undefined;
        $scope.loadPacote(0, $scope.itensporpagina);
        if ($scope.paginadorPacote != undefined)
            $scope.paginadorPacote.reiniciar();
    }

    $scope.loadPacote = function (skip, take) {
        $scope.loading = true;

        var opcoes = {
            skip: skip,
            take: take,
            codigo: $scope.campos.codigo,
            desc: $scope.campos.desc,
        };

        Api.PrecoPacote.listPage(opcoes, function (data) {
            $scope.list = data.results;
            $scope.total = data.count;
            $scope.loading = false;
        },
        function (response) {
            $scope.loading = false;
        });
    }

}]);
