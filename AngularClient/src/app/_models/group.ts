export default interface Group {
    name: string
    connections: Connection[]
}

interface Connection {
    connectionId: string
    userName: string
}