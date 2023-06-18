from django.db import models


class AssetBundles(models.Model):
    name = models.CharField(max_length=255)
    destination_os = models.CharField(max_length=255)
    file = models.FileField(upload_to='dlc/asset_bundles')
    crc = models.CharField(max_length=255)

    def delete(self, using=None, keep_parents=False):
        self.file.delete()
        super().delete(using, keep_parents)

