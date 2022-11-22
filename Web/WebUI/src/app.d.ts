declare module '@fortawesome/pro-solid-svg-icons/index.es' {
    export * from '@fortawesome/pro-solid-svg-icons';
}
declare module '@paypal/paypal-js/index.es' {
    export * from '@paypal/paypal-js'
}

declare global{
     interface Window {
        handleCredentialResponse: (response: any) => void;
     }
}