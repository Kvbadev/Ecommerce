<script lang="ts">

import dropin from 'braintree-web-drop-in';
import { agent } from "../../Utils/agent";

import { onMount } from "svelte";
import { client, hostedFields } from 'braintree-web';
  import Loader from '../Common/Loader.svelte';

let fields: {
    cardholderName: HTMLElement,
    number: HTMLElement,
    expirationDate: HTMLElement,
    cvv: HTMLElement
} = {} as any;
let loading = true, submitting, tokenize;

onMount(async () => {
    const token = await agent.PaymentGateway.GetToken();

    //return an instance of hostedFileds that are furher used to obtain a transaction nonce
    const instance = await client.create({
        authorization: token
    }).then(cl => {
        return hostedFields.create( {
            client: cl,
            authorization: token,
            fields: {
                cardholderName: {
                    container: fields.cardholderName,
                    placeholder: 'Name as it appears on your card'
                },
                number: {
                    container: fields.number,
                    placeholder: '4111 1111 1111 1111'
                },
                expirationDate: {
                    container: fields.expirationDate,
                    placeholder: '02/22'
                },
                cvv: {
                    container: fields.cvv,
                    placeholder: '123'
                }
            }
        }).then(fields => {
            loading = false;
            return fields;
        })
    })
    tokenize = e => {
        e.preventDefault();

        instance.tokenize((err, payload) => {
            if(err){
                console.error(err);
                return;
            }
            //payload = nonce to process the transaction
            console.log(payload);
        })
    }
}) 

</script>

{#if loading}
<Loader />
{/if}

<div class="container">
    <form class="form">
        <div class="cardholder-name-div">
            <label for="cardholder-name">Cardholder Name</label>
            <div class="cardholder-name" bind:this={fields.cardholderName}></div>
        </div>
        <div class="card-number-div">
            <label for="card-number">Credit card number</label>
            <div class="card-number" bind:this={fields.number}></div>
        </div>
        <div class="exp-cvv-div">
            <label for="expiration">Expiration</label>
            <div class="expiration" bind:this={fields.expirationDate}></div>
            <label for="cvv">CVV</label>
            <div class="cvv" bind:this={fields.cvv}></div>
        </div>
        <button class="submit" on:click={(e) => tokenize(e)}>Submit</button>
    </form>
</div>


<style>
    .container {
        width: auto;
        height: auto;
    }
    form {
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 35%;
    }
    form > div {
        width: 45rem;
        justify-content: center;
        height: 6rem;
        display: flex;
    }
    .submit {
        width: 15rem;
        height: 3rem;
        margin-top: 1rem;
    }
    form > div > div {
        border: 0.2rem black solid;
        padding: 1rem;
        display: grid;
    }
    form > div > label {
        width: 12rem;
        display: flex;
        justify-content: center;
        align-items: center;
    }
    form * {
        margin: 0.3rem;
    }
</style>