export interface Import {
    id?: string;
    fileName: string;
    status: string;
    totalRows: number;
    succesRows: number;
    failedRows: number;
    createdAt?: string;
    finishedAt?: string;
    totalProcessedRows?: number;
}
