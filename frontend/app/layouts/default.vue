<template>
  <div :style="maxHeightStyle" class="layout">
    <app-header style="width: 100%" />
    <div class="content">
      <slot />
    </div>
  </div>
</template>

<script lang="ts" setup>
const route = useRoute();
const limitHeight = route.meta.layoutMeta?.limitHeightToViewport ?? false;

const maxHeightStyle = computed(() =>
  limitHeight ? { height: "100dvh", overflow: "hidden" } : {}
);
</script>

<style scoped>
.layout {
  display: flex;
  flex-direction: column;
  align-items: center;

  min-height: 100dvh;
}

.content {
  flex: 1;
  width: 100%;
  max-width: 900px;
  padding: 0 var(--spacing-sm);
}

@media (max-width: 600px) {
}

@media (min-width: 601px) and (max-width: 900px) {
  .content {
    max-width: 700px;
  }
}

@media (min-width: 901px) {
  .content {
    max-width: 900px;
  }
}
</style>
