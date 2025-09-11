<template>
    <div>
        <strong>{{ title }}</strong>
        <br>
        <span>{{ description }}</span>
    </div>
</template>

<script>
    export default {
        props: {
            title: {
                type: String,
                required: true,
            },
            description: {
                type: String,
                required: true,
            },
        },
    };
</script>
