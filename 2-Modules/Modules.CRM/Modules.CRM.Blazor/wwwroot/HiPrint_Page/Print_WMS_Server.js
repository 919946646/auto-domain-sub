export function init_hiprint() {
    if (document.readyState == 'complete') {
        hiprint.init();
    }
}

//打印入库不干胶贴纸
export function print_data(ApiData, printer_name) {
    //printer_name= 'Deli DL-720C'
    //指定打印机，*** 需要启动hiprint提供的服务跳过预览
    //为后台传入用户自定义打印机做准备
    //printer_name= 'Deli DL-720C'
    //let printer = { printer: printer_name, title: '打印' }
    let printTemplateJson = {
        "panels": [{
            "index": 0,
            "height": 30,
            "width": 50,
            "paperHeader": 49.5,
            "paperFooter": 85.03937007874016,
            "printElements": [{
                "options": {
                    "left": 81,
                    "top": 18,
                    "height": 48,
                    "width": 54,
                    "field": "ID",
                    "testData": "012345678987654321",
                    "textType": "qrcode"
                },
                "printElementType": {
                    "type": "text"
                }
            }, {
                "options": {
                    "left": 6,
                    "top": 6,
                    "height": 9.75,
                    "width": 126,
                    "field": "ITEM_NO",
                    "testData": "图号",
                    "fontSize": 11.25
                },
                "printElementType": {
                    "type": "text"
                }
            }, {
                "options": {
                    "left": 6,
                    "top": 22.5,
                    "height": 9.75,
                    "width": 72,
                    "field": "GRADE",
                    "testData": "规格",
                    "fontSize": 11.25
                },
                "printElementType": {
                    "type": "text"
                }
            }, {
                "options": {
                    "left": 6,
                    "top": 43.5,
                    "height": 9.75,
                    "width": 72,
                    "field": "ITEM_NAME",
                    "testData": "名称",
                    "fontSize": 11.25
                },
                "printElementType": {
                    "type": "text"
                }
            }, {
                "options": {
                    "left": 6,
                    "top": 66,
                    "height": 9.75,
                    "width": 124.5,
                    "field": "STOCK_ADD_QUANTITY",
                    "testData": "数量",
                    "fontSize": 11.25
                },
                "printElementType": {
                    "type": "text"
                }
            }],
            "paperNumberLeft": 111,
            "paperNumberTop": 63,
            "paperNumberDisabled": true
        }]
    };
    let hiprintTemplate = new hiprint.PrintTemplate({
        template: printTemplateJson
    });
    //var postData = {
    //    ID: "1212",
    //    ORDER_NO: "121212",
    //    WORK_NO: "222",
    //    ITEM_NO: "222",
    //    ITEM_NAME: "黄磊",
    //    PHOTO: "",
    //    PHOTO_TYPE: "",
    //    STOCK_ADD_QUANTITY: 0,
    //    STOCK_SPACE_CODE: "",
    //    NOTES: "",
    //    STORE_NO: "",
    //}

    //处理数据
    let printData = JSON.parse(ApiData)
    printData.forEach(s => s.STOCK_ADD_QUANTITY = "数量：" + s.STOCK_ADD_QUANTITY); //增加数量汉字

    if (typeof printer_name === 'undefined' || printer_name == null || printer_name === '') {
        hiprintTemplate.print(printData);
    } else {
        //print2可跳过预览
        hiprintTemplate.print2(printData, printer);
    }
}

//打印出库申请请领单
export function PrintStockTicket(ApiData, printer_name) {
    let printTemplateJson = {
        "panels": [
            {
                "index": 0,
                "height": 297,
                "width": 210,
                "paperHeader": 45,
                "paperFooter": 780,
                "printElements": [
                    {
                        "options": {
                            "left": 465,
                            "top": 9,
                            "height": 66,
                            "width": 120,
                            "title": "二维码",
                            "field": "TICKET_NO",
                            "testData": "TICKET_NO",
                            "textType": "qrcode"
                        },
                        "printElementType": {
                            "type": "text"
                        }
                    },
                    {
                        "options": {
                            "left": 303,
                            "top": 28.5,
                            "height": 9.75,
                            "width": 147,
                            "title": "申请时间",
                            "field": "CREATETIME",
                            "testData": "CREATETIME",
                            "fontSize": 12
                        },
                        "printElementType": {
                            "type": "text"
                        }
                    },
                    {
                        "options": {
                            "left": 133.5,
                            "top": 28.5,
                            "height": 9.75,
                            "width": 160.5,
                            "title": "票据号",
                            "field": "TICKET_NO",
                            "testData": "TICKET_NO",
                            "fontSize": 12
                        },
                        "printElementType": {
                            "type": "text"
                        }
                    },
                    {
                        "options": {
                            "left": 21,
                            "top": 37.5,
                            "height": 13.5,
                            "width": 99,
                            "title": "出库清单",
                            "fontSize": 18
                        },
                        "printElementType": {
                            "type": "text"
                        }
                    },
                    {
                        "options": {
                            "left": 133.5,
                            "top": 52.5,
                            "height": 9.75,
                            "width": 159,
                            "title": "库房号",
                            "field": "STOCK_NO",
                            "testData": "STOCK_NO",
                            "fontSize": 12
                        },
                        "printElementType": {
                            "type": "text"
                        }
                    },
                    {
                        "options": {
                            "left": 303,
                            "top": 52.5,
                            "height": 9.75,
                            "width": 145.5,
                            "title": "申请人",
                            "field": "CREATEUID",
                            "testData": "CREATEUID",
                            "fontSize": 12
                        },
                        "printElementType": {
                            "type": "text"
                        }
                    },
                    {
                        "options": {
                            "left": 18,
                            "top": 88,
                            "height": 38,
                            "width": 560,
                            "textAlign": "center",
                            "columns": [
                                {
                                    "columns": [
                                        {
                                            "title": "工号",
                                            "field": "WORK_NO",
                                            "width": 100,
                                            "colspan": 1,
                                            "rowspan": 1,
                                            "checked": true
                                        },
                                        {
                                            "title": "图号",
                                            "field": "ITEM_NO",
                                            "width": 100,
                                            "colspan": 1,
                                            "rowspan": 1,
                                            "checked": true
                                        },
                                        {
                                            "title": "名称",
                                            "field": "ITEM_NAME",
                                            "width": 100,
                                            "colspan": 1,
                                            "rowspan": 1,
                                            "checked": true
                                        },
                                        {
                                            "title": "数量",
                                            "field": "QTY_REQ",
                                            "width": 50,
                                            "colspan": 1,
                                            "rowspan": 1,
                                            "checked": true
                                        },
                                        {
                                            "title": "货位",
                                            "field": "STOCK_SPACE_CODE",
                                            "width": 65,
                                            "colspan": 1,
                                            "rowspan": 1,
                                            "checked": true
                                        },
                                        {
                                            "title": "单重",
                                            "field": "WEIGHT",
                                            "width": 50,
                                            "colspan": 1,
                                            "rowspan": 1,
                                            "checked": true
                                        },
                                        {
                                            "title": "说明",
                                            "field": "NOTES",
                                            "width": 90,
                                            "colspan": 1,
                                            "rowspan": 1,
                                            "checked": true
                                        }
                                    ]
                                }
                            ]
                        },
                        "printElementType": {
                            "title": "表格",
                            "type": "tableCustom",
                            "field": "table"
                        }
                    },
                    {
                        "options": {
                            "left": 28.5,
                            "top": 795,
                            "height": 9.75,
                            "width": 120,
                            "title": "这是页尾内容"
                        },
                        "printElementType": {
                            "title": "文本",
                            "type": "text"
                        }
                    }
                ],
                "paperNumberLeft": 565.5,
                "paperNumberTop": 819
            }
        ]
    }
    let hiprintTemplate = new hiprint.PrintTemplate({
        template: printTemplateJson
    });

    //处理数据
    let printData = JSON.parse(ApiData)
    //printData.forEach(s => s.STOCK_ADD_QUANTITY = "数量：" + s.STOCK_ADD_QUANTITY); //增加数量汉字

    if (typeof printer_name === 'undefined' || printer_name == null || printer_name === '') {
        hiprintTemplate.print(printData);
    } else {
        //print2可跳过预览
        hiprintTemplate.print2(printData, printer);
    }
}