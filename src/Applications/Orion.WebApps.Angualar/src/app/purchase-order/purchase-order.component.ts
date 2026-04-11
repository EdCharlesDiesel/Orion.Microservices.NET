import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { jqxGridComponent } from 'jqwidgets-scripts/jqwidgets-ts/angular_jqxgrid';

// Declare jqx / jqwidgets globals
declare let jqx: any;
declare let jqwidgets: any;

@Component({
  selector: 'app-purchase-order',
  templateUrl: './purchase-order.component.html',
  styleUrls: ['./purchase-order.component.css']
})
export class PurchaseOrderComponent implements OnInit, AfterViewInit {

  private readonly BASE_URL = environment.API_ENDPOINT;

  @ViewChild('IDPurchaseOrderHeaderGrid', { static: false })
  IDPurchaseOrderHeaderGrid!: jqxGridComponent;

  IDPurchaseOrderHeaderGridSource!: jqwidgets.GridSource;
  IDPurchaseOrderHeaderGridOptions!: jqwidgets.GridOptions;

  constructor() { }

  ngOnInit(): void { }

  ngAfterViewInit(): void {
    this.IDPurchaseOrderHeaderGridSource = {
      datatype: 'json',
      url: `${this.BASE_URL}Purchasing/GetPurchaseOrderHeader/`,
      sortcolumn: 'PurchaseOrderID',
      sortdirection: 'asc',
      id: 'PurchaseOrderID',
      pagesize: 15,
      datafields: [
        { name: 'PurchaseOrderID', type: 'int' },
        { name: 'OrderDate', type: 'string' },
        { name: 'RevisionNumber', type: 'int'},
        { name: 'Status', type: 'string' },
        { name: 'PlacedBy', type: 'string' },
        { name: 'VendorID', type: 'int' },
        { name: 'ShippedThrough', type: 'string' },
        { name: 'SubTotal', type: 'float' },
        { name: 'Freight', type: 'float' },
        { name: 'TaxAmt', type: 'float' },
        { name: 'TotalDue', type: 'float' }
      ],
      sort: () => this.IDPurchaseOrderHeaderGrid.updatebounddata('sort'),
      filter: () => this.IDPurchaseOrderHeaderGrid.updatebounddata('filter'),
    };

    this.IDPurchaseOrderHeaderGridOptions = {
      width: 1050,
      pagesizeoptions: ['5', '10', '15'],
      theme: 'office',
      pageable: true,
      sortable: true,
      filterable: true,
      autoheight: true,
      virtualmode: true,
      enabletooltips: true,
      rendergridrows: (obj: any) => obj.data,
      source: new jqx.dataAdapter(this.IDPurchaseOrderHeaderGridSource, {
        beforeSend: (jqxhr: any, settings: any) => { },
        loadError: (xhr: any, status: any, error: any) => console.error(error)
      }),
      columns: [
        { text: 'Order ID', datafield: 'PurchaseOrderID', width: 100, cellsalign: 'center' },
        { text: 'Order Date', datafield: 'OrderDate', width: 100, cellsalign: 'center' },
        { text: 'Rev. No', datafield: 'RevisionNumber', width: 50, cellsalign: 'center', filterable: false },
        { text: 'Status', datafield: 'Status', width: 75 },
        { text: 'Placed By', datafield: 'PlacedBy', width: 80 },
        { text: 'Vendor ID', datafield: 'VendorID', width: 75, cellsalign: 'center' },
        { text: 'Shipped Through', datafield: 'ShippedThrough', width: 130 },
        { text: 'Sub. Total', datafield: 'SubTotal', width: 100, cellsalign: 'center' },
        { text: 'Freight.', datafield: 'Freight', width: 100, cellsalign: 'center' },
        { text: 'Tax Amt.', datafield: 'TaxAmt', width: 100, cellsalign: 'center' },
        { text: 'Total Due', datafield: 'TotalDue', width: 100, cellsalign: 'center' }
      ],
      rowdetails: true,
      rowdetailstemplate: {
        rowdetails: '<div id="IDPurchaseOrderDetailer"></div>',
        rowdetailsheight: 250,
      },
      initrowdetails: (index: number, parentElement: any, gridElement: any, record: any): void => {
        if (parentElement.children[0]) {
          const PurchaseOrderDetailerGridSource: jqwidgets.GridSource = {
            datatype: 'json',
            url: `${environment.API_ENDPOINT}Purchasing/GetPurchaseOrderDetailer/${record.PurchaseOrderID}`,
            sortcolumn: 'PurchaseOrderDetailId',
            sortdirection: 'asc',
            id: 'PurchaseOrderDetailId',
            pagesize: 5,
            datafields: [
              { name: 'PurchaseOrderDetailId', type: 'int' },
              { name: 'ProductId', type: 'int' },
              { name: 'Name', type: 'string' },
              { name: 'UnitPrice', type: 'float' },
              { name: 'OrderQty', type: 'int' },
              { name: 'LineTotal', type: 'float' },
              { name: 'ReceivedQty', type: 'float' },
              { name: 'RejectedQty', type: 'float' },
              { name: 'StockedQty', type: 'float' }
            ]
          };

          const PurchaseOrderDetailerGridOptions: jqwidgets.GridOptions = {
            width: 900,
            pagesizeoptions: ['5', '10', '15'],
            theme: 'office',
            pageable: true,
            sortable: true,
            filterable: false,
            autoheight: true,
            virtualmode: true,
            enabletooltips: true,
            rendergridrows: (obj: any) => obj.data,
            source: new jqx.dataAdapter(PurchaseOrderDetailerGridSource, {
              beforeSend: (jqxhr: any, settings: any) => { },
              loadError: (xhr: any, status: any, error: any) => console.error(error)
            }),
            columns: [
              { text: 'Order Detail ID', datafield: 'PurchaseOrderDetailId', width: 130, cellsalign: 'center' },
              { text: 'Product ID', datafield: 'ProductId', width: 100, cellsalign: 'center' },
              { text: 'Name', datafield: 'Name', width: 150 },
              { text: 'Unit Price', datafield: 'UnitPrice', width: 100, cellsalign: 'center' },
              { text: 'Order Qty', datafield: 'OrderQty', width: 100, cellsalign: 'center' },
              { text: 'Line Total', datafield: 'LineTotal', width: 100, cellsalign: 'center' },
              { text: 'Received Qty', datafield: 'ReceivedQty', width: 100, cellsalign: 'center' },
              { text: 'Rejected Qty', datafield: 'RejectedQty', width: 100, cellsalign: 'center' },
              { text: 'Stocked Qty', datafield: 'StockedQty', width: 100, cellsalign: 'center' }
            ]
          };

          jqwidgets.createInstance(parentElement.children[0], 'jqxGrid', PurchaseOrderDetailerGridOptions);
        }
      }
    };

    this.IDPurchaseOrderHeaderGrid.createComponent(this.IDPurchaseOrderHeaderGridOptions);
  }
}
