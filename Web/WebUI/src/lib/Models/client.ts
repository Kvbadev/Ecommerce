export default interface Client {
    username: string;
    privileges: Array<string>;
    money_spent: number;
    createdAt: string;
}