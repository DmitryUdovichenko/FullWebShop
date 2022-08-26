import { IAddress } from "./address";
import { IOrderItem } from "./orderItem";

export interface IOrderResponse {
    id: number;
    userEmail: string;
    createdDate: Date;
    shippingAddress: IAddress;
    deliveryMethod: string;
    deliveryPrice: number;
    orderItems: IOrderItem[];
    subtotal: number;
    total: number;
    status: string;
}