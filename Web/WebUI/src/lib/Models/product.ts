export default interface Product {
    id: string;
    name: string;
    description: string;
    price: number;
    photos: Array<string> | string;
}