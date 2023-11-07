interface Connection {
    connectionId: string
    userName: string
}

export interface Group {
    name: string
    connections: Connection[]
}
