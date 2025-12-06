declare module "#app" {
  interface PageMeta {
    layoutMeta: {
      limitHeightToViewport: boolean;
    };
  }
}

// It is always important to ensure you import/export something when augmenting a type
export {};
