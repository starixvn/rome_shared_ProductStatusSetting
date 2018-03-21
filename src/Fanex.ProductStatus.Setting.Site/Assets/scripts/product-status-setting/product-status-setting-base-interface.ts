//interface IProductStatusSettingBase {
//    $: JQueryStatic;
//    $resources: any;
//    $errorCodeDefault: number;
//    $timeoutDefault: number;
//    $thead: JQuery;
//    $tbody: JQuery;

//    $custId: number;
//    $custRole: number;
//    $productId: number;

//    disableElement(element: HTMLElement);
//    enableElement(element: HTMLElement);

//    getNextLabel: (element: HTMLElement) => HTMLElement;
//    allowActionOnElement: (element: HTMLElement) => boolean;
//    isEnabledElement: (statusSpan: HTMLElement) => boolean;
//    getRoleId: (element: HTMLElement) => number;

//    isEnablebUpline: (element: HTMLElement) => boolean;
//    getBetType: (element: HTMLElement) => number;
//    getSportType: (element: HTMLElement) => number;
//    getTotalStatus: (element: HTMLElement) => number;

//    isChecked: (element: HTMLElement) => boolean;
//    isChangeState: (originValue: number, currentValue: number) => boolean;
//    buildElementId: (className: string, roleId: number, betType: number) => string; 
//    parseInt: (value: string) => number;
//    parseBoolean: (value: string) => boolean;

//    //onSubmit(e: any);
//    //setupAntiXSRF();
//    callAjax: (headers: any, updateViewModel: any, action: string) => void;

//    onSuccess: (data: any) => void;
//    onError: () => void;
//    showMessage: (type: any, message: string) => void;
//    hideMessage: () => void;
//}
