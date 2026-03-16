export enum ImportStatus {
    COMPLETED = 0,
    PENDING = 1,
    PROCESSING = 2,
    FAILED = 3
}

export function getStatusByNumber(value: number) {
    switch (value) {
        case 0:
            return ImportStatus.COMPLETED;
        case 1:
            return ImportStatus.PENDING;
        case 2  :
            return ImportStatus.PROCESSING;
        case 3:
            return ImportStatus.FAILED;
        default:
            return ImportStatus.PENDING;    
    }
}

export function getBadgeColorByImportStatus(value: ImportStatus) {
    switch (value) {
        case ImportStatus.COMPLETED:
            return "success";
        case ImportStatus.PENDING:
            return "warning";
        case ImportStatus.PROCESSING:
            return "info";
        case ImportStatus.FAILED:
            return "danger";
    }
}
