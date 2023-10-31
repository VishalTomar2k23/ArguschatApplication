export interface Message{
    senderId : number;
    receiverId : number;
    content : string;
    isSeen : boolean;
    dateTime : Date;
    type : string;
    replyedToId: number;
    isReply : boolean;
}