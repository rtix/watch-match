<template>
  <div :style="maxHeightStyle">
    <app-header />
    <div class="content">
      <slot />
    </div>
    <!-- <app-footer/> -->
  </div>
</template>

<script lang="ts" setup>
const route = useRoute();
const limitHeight = route.meta.layoutMeta.limitHeightToViewport ?? false;

const maxHeightStyle = computed(() =>
  limitHeight ? { maxHeight: "100dvh", overflow: "hidden" } : {}
);
</script>

<style scoped>
.content {
  max-width: 900px;
  margin: 0 auto;
  padding: 0 0.5rem;
}

@media (max-width: 600px) {
  .content {
    /* padding: 0 15px; */
  }
}

/* Medium devices (tablets) */
@media (min-width: 601px) and (max-width: 900px) {
  .content {
    max-width: 700px;
  }
}

/* Large devices (desktop) */
@media (min-width: 901px) {
  .content {
    max-width: 900px;
  }
}
</style>
