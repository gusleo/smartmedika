import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';

import {
  MdAutocompleteModule,
  MdButtonModule,
  MdButtonToggleModule,
  MdCardModule,
  MdCheckboxModule,
  MdChipsModule,
  MdCoreModule,
  MdTableModule,
  MdDatepickerModule,
  MdDialogModule,
  MdExpansionModule,
  MdGridListModule,
  MdIconModule,
  MdInputModule,
  MdListModule,
  MdMenuModule,
  MdNativeDateModule,
  MdPaginatorModule,
  MdProgressBarModule,
  MdProgressSpinnerModule,
  MdRadioModule,
  MdRippleModule,
  MdSelectModule,
  MdSidenavModule,
  MdSliderModule,
  MdSlideToggleModule,
  MdSnackBarModule,
  MdSortModule,
  MdTabsModule,
  MdToolbarModule,
  MdTooltipModule,
  StyleModule,
  FullscreenOverlayContainer,
  MdSelectionModule,
  OverlayContainer
} from '@angular/material';
import { CdkTableModule } from '@angular/cdk';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';

import 'hammerjs';

import { MaterialRoutes } from './material.routing';
import { ButtonsComponent } from './buttons/buttons.component';
import { CardsComponent } from './cards/cards.component';
import { InputComponent } from './input/input.component';
import {
  CheckboxComponent,
  MdCheckboxDemoNestedChecklistComponent } from './checkbox/checkbox.component';
import { RadioComponent } from './radio/radio.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { ListsComponent } from './lists/lists.component';
import { GridComponent } from './grid/grid.component';
import { ProgressComponent } from './progress/progress.component';
import { TabsComponent } from './tabs/tabs.component';
import { ToggleComponent } from './toggle/toggle.component';
import { TooltipComponent } from './tooltip/tooltip.component';
import { MenuComponent } from './menu/menu.component';
import { SliderComponent } from './slider/slider.component';
import { SnackbarComponent } from './snackbar/snackbar.component';
import {
  DialogComponent,
  ContentElementDialogComponent,
  IFrameDialogComponent,
  JazzDialogComponent } from './dialog/dialog.component';
import { SelectComponent } from './select/select.component';
import { AutocompleteComponent } from './autocomplete/autocomplete.component';
import { ChipsComponent } from './chips/chips.component';
import { DatepickerComponent } from './datepicker/datepicker.component';

import { ExpansionComponent } from './expansion/expansion.component';
import { TableComponent } from './table/table.component';
import { PeopleDatabase } from './table/people-database';
import { TableHeaderDemoComponent } from './table/table-header-demo';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(MaterialRoutes),
    MdAutocompleteModule,
    MdButtonModule,
    MdButtonToggleModule,
    MdCardModule,
    MdCheckboxModule,
    MdChipsModule,
    MdTableModule,
    MdDatepickerModule,
    MdDialogModule,
    MdExpansionModule,
    MdGridListModule,
    MdIconModule,
    MdInputModule,
    MdListModule,
    MdMenuModule,
    MdCoreModule,
    MdPaginatorModule,
    MdProgressBarModule,
    MdProgressSpinnerModule,
    MdRadioModule,
    MdRippleModule,
    MdSelectModule,
    MdSidenavModule,
    MdSlideToggleModule,
    MdSliderModule,
    MdSnackBarModule,
    MdSortModule,
    MdTabsModule,
    MdToolbarModule,
    MdTooltipModule,
    MdNativeDateModule,
    StyleModule,
    MdNativeDateModule,
    MdSelectionModule,
    HttpModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    CdkTableModule
  ],
  providers: [
    {provide: OverlayContainer, useClass: FullscreenOverlayContainer},
    PeopleDatabase
  ],
  declarations: [
    ButtonsComponent,
    CardsComponent,
    InputComponent,
    CheckboxComponent,
    MdCheckboxDemoNestedChecklistComponent,
    RadioComponent,
    ToolbarComponent,
    ListsComponent,
    GridComponent,
    ProgressComponent,
    TabsComponent,
    ToggleComponent,
    TooltipComponent,
    MenuComponent,
    SliderComponent,
    SnackbarComponent,
    DialogComponent,
    SelectComponent,
    AutocompleteComponent,
    ChipsComponent,
    DatepickerComponent,
    ContentElementDialogComponent,
    IFrameDialogComponent,
    JazzDialogComponent,
    ExpansionComponent,
    TableComponent,
    TableHeaderDemoComponent
  ],
  entryComponents: [
    ContentElementDialogComponent,
    IFrameDialogComponent,
    JazzDialogComponent
  ],
})

export class MaterialComponentsModule {}
