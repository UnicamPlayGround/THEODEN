from django.db import models

from Tencu_Framework_Server.models.asset_bundles.AssetBundles import AssetBundles


class Dependency(models.Model):
    parent = models.ForeignKey(AssetBundles, on_delete=models.CASCADE, related_name='parent')
    child = models.ForeignKey(AssetBundles, on_delete=models.CASCADE, related_name='child')
