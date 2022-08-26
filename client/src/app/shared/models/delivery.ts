export interface IDeliveryMethod {
    id: number;
    createdBy: string;
    createdDate: Date;
    lastMidofiedBy: string;
    lastMidofiedDate: Date;
    name: string;
    deliveryTime: string;
    description: string;
    price: number;
}