export default interface Transaction {
    price: number;
    products: Array<string>;
    issuedAt: Date;
    Success: boolean;
}