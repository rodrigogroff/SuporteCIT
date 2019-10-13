import React from "react";
import { Switch, Route, Redirect } from "react-router";
import { TransitionGroup, CSSTransition } from "react-transition-group";
import Hammer from "rc-hammerjs";

import Header from "../Header";

import AssociadoQRCODE from "../../pages/associado/QRCODE/QRCODE";
import AssociadoLimites from "../../pages/associado/limites/Limites";
import AssociadoExtratos from "../../pages/associado/extratos/Extratos";
import AssociadoParcelamentos from "../../pages/associado/parcelamentos/Parcelamentos";
import AssociadoFaturas from "../../pages/associado/faturas/Faturas";

import LojistaVenda from "../../pages/lojista/venda/venda";

import s from "./Layout.module.scss";

export default class LayoutComponent extends React.Component {

  render() {
    return (
      <div
        className={[
          s.root,
          "sidebar-" + this.props.sidebarPosition,
          "sidebar-" + this.props.sidebarVisibility
        ].join(" ")}
      >
        <div className={s.wrap}>
          <Header
            mainVars={this.props.mainVars}
            updateMainVars={this.props.updateMainVars}
          />
          <Hammer onSwipe={this.handleSwipe}>
            <main className={s.content}>
              <TransitionGroup>
                <CSSTransition key={this.props.location.pathname} classNames="fade" timeout={200}>
                  <Switch>
                    <Route path="/app/associado/QRCODE" exact mainVars={this.props.mainVars} updateMainVars={this.props.updateMainVars} component={AssociadoQRCODE} />
                    <Route path="/app/associado/limites" exact mainVars={this.props.mainVars} updateMainVars={this.props.updateMainVars} component={AssociadoLimites} />
                    <Route path="/app/associado/extratos" exact mainVars={this.props.mainVars} updateMainVars={this.props.updateMainVars} component={AssociadoExtratos} />
                    <Route path="/app/associado/parcelamentos" exact mainVars={this.props.mainVars} updateMainVars={this.props.updateMainVars} component={AssociadoParcelamentos} />
                    <Route path="/app/associado/faturas" exact mainVars={this.props.mainVars} updateMainVars={this.props.updateMainVars} component={AssociadoFaturas} />
                    <Route path="/app/lojista/venda" exact mainVars={this.props.mainVars} updateMainVars={this.props.updateMainVars} component={LojistaVenda} />
                  </Switch>
                </CSSTransition>
              </TransitionGroup>
            </main>
          </Hammer>
        </div>
      </div>
    );
  }
}
