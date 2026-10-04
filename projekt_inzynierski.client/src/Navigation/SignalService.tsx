import * as signalR from "@microsoft/signalr";

let connection: signalR.HubConnection | null = null;
const API_URL = import.meta.env.VITE_API_URL;
export const startConnection = async (token?: string) => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl(`${API_URL}/notificationHub`, {
            accessTokenFactory: () => token ?? "",
            withCredentials: true
        })
        .withAutomaticReconnect()
        .build();

    try {
        await connection.start();
        console.log("SignalR connected");
    } catch (err) {
        console.error("SignalR error: ", err);
    }
};

export const onNotification = (callback: (message: string) => void) => {
    connection?.on("ReceiveNotification", callback);
};

export const stopConnection = async () => {
    if (connection) {
        await connection.stop();
        connection = null;
    }
};
