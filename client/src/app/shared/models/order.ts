import { IAddress } from "./address";

export interface IOrderCreate {
    basketId: string;
    deliveryMethodId: number;
    address: IAddress;
}