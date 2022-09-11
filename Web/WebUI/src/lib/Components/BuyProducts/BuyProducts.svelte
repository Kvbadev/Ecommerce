<script lang="ts">
import { onMount } from "svelte";
import {client, hostedFields} from "braintree-web";
import { agent } from "../../Utils/agent";
import PaymentForm from "./PaymentForm.svelte";


onMount(async () => {
    const instance = await client.create({
        authorization: await agent.PaymentGateway.GetToken()
    }).then(cl => {
        // hostedFields.create({
        //     client: cl,
        //     styles: {

        //     }
        // })
    });
})
</script>

<h1>Buy Products</h1>
<PaymentForm />